using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Player
{
    public class LocalMultiplayerSnakeTail : NetworkBehaviour, ISnakeTail
    {
        private NetworkObject _prefab;
        private SnakeTail _tail;

        private readonly List<Action<SnakeTailCalculationData>> _calculationDataWaitingList = new();
        
        public event Action<SnakePoint> AddNewPoint;

        public void SetSnake(SnakeTail tail)
        {
            _tail = tail;
        }
        
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

        public void CalculateDataForAddLength(Action<SnakeTailCalculationData> onCalculationData)
        {
            _calculationDataWaitingList.Add(onCalculationData);
            CalculateDataForAddLengthClientRpc();
        }

        [Rpc(SendTo.Owner)]
        private void CalculateDataForAddLengthClientRpc()
        {
            var data = _tail.CalculateDataForAddLength();
            CalculateDataForAddLengthServerRpc(data.LastPoint);
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
        private void CalculateDataForAddLengthServerRpc(Vector3 lastPoint)
        {
            var data = new SnakeTailCalculationData(lastPoint);
            
            foreach (var i in _calculationDataWaitingList)
            {
                i.Invoke(data);
            }
            
            _calculationDataWaitingList.Clear();
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