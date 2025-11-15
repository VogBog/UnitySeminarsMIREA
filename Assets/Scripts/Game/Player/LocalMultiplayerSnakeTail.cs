using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Game.Player
{
    public class LocalMultiplayerSnakeTail : NetworkBehaviour, ISnakeTail
    {
        private NetworkObject _prefab;
        
        public event Action<SnakePoint> AddNewPoint; 
        
        public void SetPrefab(SnakePoint prefab)
        {
            _prefab = prefab.GetComponent<NetworkObject>();
            if (_prefab == null)
            {
                Debug.LogError("SnakeTail must have NetworkObject");
            }
        }
        
        public void Instantiate(Vector3 position, Quaternion rotation, Action<SnakePoint> onSpawn)
        {
            var obj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                _prefab,
                OwnerClientId,
                true,
                position: position,
                rotation: rotation);

            ulong id = obj.NetworkObjectId;
            
            if(obj.TryGetComponent(out SnakePoint point))
                onSpawn.Invoke(point);
            
            InstantiateClientRpc(id);
        }

        public void Despawn(SnakePoint point)
        {
            var networkObject = point.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }

        [ClientRpc]
        private void InstantiateClientRpc(ulong objectId)
        {
            if (!IsOwner)
                return;

            StartCoroutine(InstantiateRoutine(objectId));
        }

        private IEnumerator InstantiateRoutine(ulong objectId)
        {
            for (int attempt = 0; attempt < 1_000; attempt++)
            {
                if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var networkObject))
                {
                    yield return null;
                    continue;
                }

                if (!networkObject.TryGetComponent(out SnakePoint tail))
                {
                    yield return null;
                    continue;
                }
                
                AddNewPoint?.Invoke(tail);
                break;
            }
        }
    }
}