using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using SceneObjects.NetworkComponents.SyncedOwnerDetector;
using Unity.Netcode;
using UnityEngine;

namespace Pool
{
    public class LocalMultiplayerObjectPoolInstantiator : NetworkBehaviour, IObjectPoolInstantiator
    {
        private IEditablePool _pool;
        private ObjectPool _totalPool;

        private readonly List<(Type, Action<Component>)> _waitingList = new();
        
        public void Initialize(IEditablePool pool)
        {
            _pool = pool;
            _totalPool = this.FindFirstObjectByTypeOrException<ObjectPool>();
        }
        
        public void InstantiateObject(Type type, PooledPrefab prefab, Transform parent, Action<Component> onInstantiate)
        {
            _waitingList.Add((type, onInstantiate));
            InstantiateObjectServerRpc(type.FullName, NetworkManager.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InstantiateObjectServerRpc(string typeName, ulong ownerId)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError($"Internal Error in ObjectPool. Cannot find type with name {typeName}");
                return;
            }

            StartCoroutine(ServerRoutine(typeName, type, ownerId));
        }

        private IEnumerator ServerRoutine(string typeName, Type type, ulong ownerId)
        {
            var prefabs = _pool.GetPrefabs();
            int attempts = 0;
            PooledPrefab prefab;
            while (!prefabs.TryGetValue(type, out prefab) && attempts < 100)
            {
                attempts++;
                yield return null;
            }

            if (attempts == 100)
            {
                Debug.LogError($"Internal Error in ObjectPool. Cannot find prefab with type {typeName}");
                yield break;
            }

            if (!prefab.Prefab.TryGetComponent<NetworkObject>(out var networkObject))
            {
                Debug.LogError("Cannot spawn prefab without NetworkObject");
                yield break;
            }

            var instance = NetworkManager.SpawnManager.InstantiateAndSpawn(
                networkObject,
                NetworkManager.LocalClientId,
                destroyWithScene: true);
            ulong id = instance.NetworkObjectId;

            InstantiateObjectClientRpc(typeName, ownerId, id);
        }

        [ClientRpc]
        private void InstantiateObjectClientRpc(string typeName, ulong ownerId, ulong objectId)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError($"Internal Error in ObjectPool. Cannot find type with name {typeName}");
                return;
            }
            
            StartCoroutine(ClientRoutine(type, ownerId, objectId));
        }

        private IEnumerator ClientRoutine(Type type, ulong ownerId, ulong objectId)
        {
            for (int i = 0; i < 10_000; i++)
            {
                if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
                {
                    yield return null;
                    continue;
                }

                if (!networkObject.TryGetComponent(type, out var component))
                    break;

                if (component is IOwnerDetectorProvider provider &&
                    provider.OwnerDetector is LocalMultiplayerOwnerDetector lmDetector)
                {
                    lmDetector.SetOwner(ownerId);
                }

                while (_pool == null)
                {
                    yield return null;
                    Debug.Log("Wait))");
                }

                if (NetworkManager.LocalClientId != ownerId)
                {
                    if (_pool.GetPrefabs().TryGetValue(type, out var prefab))
                    {
                        prefab.InstantiatedCallback?.Invoke(_totalPool, component);
                    }

                    break;
                }

                for (int j = 0; j < _waitingList.Count; j++)
                {
                    var (t, action) = _waitingList[j];
                    if (type.IsEquivalentTo(t))
                    {
                        _waitingList.RemoveAt(j);
                        action.Invoke(component);
                        break;
                    }
                }

                break;
            }
        }
    }
}