using System;
using System.Collections.Generic;
using SceneObjects.NetworkComponents.SyncedOwnerDetector;
using Unity.Netcode;
using UnityEngine;

namespace Pool
{
    public class LocalMultiplayerObjectPoolNetworkObject : NetworkBehaviour, IObjectPool, IEditablePool
    {
        private IObjectPool _pool;
        private IEditablePool _editablePool;
        private readonly Dictionary<Component, NetworkObject> _networkObjects = new();
        private readonly List<(Type, Action<Component>)> _waitingList = new();

        public void SetPool(IObjectPool pool)
        {
            _pool = pool;
            pool.SetInstantiator(new LocalMultiplayerObjectPoolInstantiator(NetworkObject));
            
            if(pool is IEditablePool editablePool)
                _editablePool = editablePool;
        }

        public void SetInstantiator(IObjectPoolInstantiator instantiator) => _pool.SetInstantiator(instantiator);

        public void RegisterPrefab(Type type, PooledPrefab prefab)
        {
            _pool.RegisterPrefab(type, prefab);
        }

        public void CreateInstances(Type type, int count)
        {
            _pool.CreateInstances(type, count);
        }

        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count)
        {
            _pool.RegisterAndInstantiatePrefab(type, prefab, count);
        }

        public void Spawn<T>(Vector3 position, Quaternion rotation, Action<T> onSpawn) where T : Component
        {
            SpawnRoutine(position, rotation, typeof(T), component =>
            {
                if (component is T t)
                    onSpawn?.Invoke(t);
            });
        }

        public void Spawn(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn)
        {
            SpawnRoutine(position, rotation, type, onSpawn);
        }

        private void SpawnRoutine(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn)
        {
            _waitingList.Add((type, component =>
            {
                onSpawn?.Invoke(component);
            }));

            var typeName = type.FullName;
            var creatorId = NetworkManager.LocalClientId;
            SpawnServerRpc(typeName, creatorId, position, rotation);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc(
            string typeName, ulong ownerId,
            Vector3 position, Quaternion rotation)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                return;
            }
            
            _pool.Spawn(position, rotation, type, component =>
            {
                ChangeOwner(component, ownerId);
                var networkObject = GetNetworkObjectFrom(component);
                SpawnClientRpc(typeName, networkObject.NetworkObjectId, ownerId, position, rotation);
                
                if(NetworkManager.LocalClientId == ownerId)
                    CheckWaitingList(type, component);
            });
        }

        [ClientRpc]
        private void SpawnClientRpc(
            string typeName, ulong instanceId, ulong ownerId,
            Vector3 position, Quaternion rotation)
        {
            if (IsServer)
                return;
            
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find type with name {typeName}");
                return;
            }

            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(instanceId, out var networkObject))
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find object with id {instanceId}");
                return;
            }

            if (!networkObject.TryGetComponent(type, out var component))
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find component with type {typeName} on instance");
                return;
            }
            
            if(_editablePool.GetPoolObjects().TryGetValue(type, out var stack))
            {
                var list = new List<Component>();
                while (stack.Count > 0)
                {
                    var c = stack.Pop();
                    if (c == component)
                    {
                        break;
                    }
                    
                    list.Add(c);
                }

                foreach (var i in list)
                {
                    stack.Push(i);
                }
            }

            if (_editablePool.GetSpawnedObjects().TryGetValue(type, out var spawnedObjects))
            {
                spawnedObjects.Add(component);
            }

            component.transform.position = position;
            component.transform.rotation = rotation;
            component.gameObject.SetActive(true);
            ChangeOwner(component, ownerId);
            
            if(NetworkManager.LocalClientId == ownerId)
                CheckWaitingList(type, component);
        }

        private void CheckWaitingList(Type type, Component component)
        {
            for (int i = 0; i < _waitingList.Count; i++)
            {
                var (t, action) = _waitingList[i];
                if (type.IsEquivalentTo(t))
                {
                    _waitingList.RemoveAt(i);
                    action.Invoke(component);
                    return;
                }
            }
        }

        private void ChangeOwner(Component component, ulong newOwner)
        {
            if (component is not IOwnerDetectorProvider provider)
                return;
            if (provider.OwnerDetector is not ILocalMultiplayerOwnerDetector lmDetector)
                return;
            lmDetector.SetOwner(newOwner);
        }

        public void Despawn<T>(T component) where T : Component
        {
            Despawn(component, typeof(T));
        }

        public void Despawn(Component component, Type type)
        {
            var typeName = type.FullName;
            var objectId = GetNetworkObjectFrom(component).NetworkObjectId;
            DespawnRpc(typeName, objectId);
        }

        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        private void DespawnRpc(string typeName, ulong objectId)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find type with name {typeName}");
                return;
            }

            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find object with id {objectId}");
                return;
            }

            if (!networkObject.TryGetComponent(type, out var component))
            {
                Debug.LogError($"Internal error in ObjectPool. Cannot find component with type {typeName} on instance");
                return;
            }
            
            _pool.Despawn(component, type);
        }

        public Dictionary<Type, Stack<Component>> GetPoolObjects() => _editablePool.GetPoolObjects();

        public Dictionary<Type, List<Component>> GetSpawnedObjects() => _editablePool.GetSpawnedObjects();

        private NetworkObject GetNetworkObjectFrom(Component component)
        {
            if (!_networkObjects.TryGetValue(component, out var result))
            {
                result = component.GetComponent<NetworkObject>();
                _networkObjects.Add(component, result);
            }
            
            if(result == null)
                Debug.LogError("Cannot spawn object without NetworkObject component", component.gameObject);

            return result;
        }
    }
}