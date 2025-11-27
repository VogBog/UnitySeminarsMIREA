using System;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Game.LocalMultiplayer
{
    public class LocalMultiplayerCarsSpawner : NetworkBehaviour, ICarsSpawner
    {
        private int _spawnedCars = 0;
        private bool _isMyCar = false;
        
        public event Action<CarMovement> MustInitialize; 
        
        public bool CanSpawnCars() => IsServer;

        public bool AddCameraAndMovement() => _isMyCar;

        public IEnumerator Instantiate(CarMovement prefab, Vector3 position, Quaternion rotation, Action<CarMovement> onSpawn)
        {
            if (!prefab.TryGetComponent<NetworkObject>(out var networkObject))
            {
                Debug.LogError("Player has no network object!");
                yield break;
            }

            var players = NetworkManager.Singleton.ConnectedClientsIds;
            ulong ownerId = players[_spawnedCars];

            _isMyCar = NetworkManager.Singleton.LocalClientId == ownerId;

            var instance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                    networkObject,
                    ownerId,
                    true,
                    true,
                    position: position,
                    rotation: rotation)
                .GetComponent<CarMovement>();
            
            onSpawn?.Invoke(instance);
            
            _spawnedCars++;

            if (_isMyCar)
                MustInitialize?.Invoke(instance);
            else
                InitializeCarClientRpc(ownerId);

            yield return null;
        }

        [ClientRpc]
        private void InitializeCarClientRpc(ulong ownerId)
        {
            if (NetworkManager.Singleton.LocalClientId != ownerId)
                return;

            _isMyCar = true;

            StartCoroutine(InitializeCarRoutine(ownerId));
        }

        private IEnumerator InitializeCarRoutine(ulong ownerId)
        {
            for (int attempt = 0; attempt < 10_000; attempt++)
            {
                var networkObject = NetworkManager.SpawnManager.PlayerObjects
                    .FirstOrDefault(x => x.OwnerClientId == ownerId);
                if (networkObject == null)
                {
                    yield return null;
                    continue;
                }

                if (!networkObject.TryGetComponent<CarMovement>(out var car))
                {
                    yield return null;
                    continue;
                }
                
                MustInitialize?.Invoke(car);
                yield break;
            }
        }
    }
}