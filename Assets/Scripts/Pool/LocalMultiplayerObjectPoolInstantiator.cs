using System;
using Unity.Netcode;
using UnityEngine;

namespace Pool
{
    public class LocalMultiplayerObjectPoolInstantiator : IObjectPoolInstantiator
    {
        private readonly NetworkObject _networkObject;

        public LocalMultiplayerObjectPoolInstantiator(NetworkObject networkObject)
        {
            _networkObject = networkObject;
        }
        
        public void InstantiateObject(Type type, PooledPrefab prefab, Transform parent, Action<Component> onInstantiate)
        {
            if (!prefab.Prefab.TryGetComponent<NetworkObject>(out var networkObject))
            {
                Debug.LogError($"Cannot instantiate object, that hasn't a NetworkObject component. {prefab.Prefab}");
                return;
            }

            var instance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                    networkObject,
                    NetworkManager.Singleton.LocalClientId,
                    true)
                .GetComponent(type);
            
            onInstantiate?.Invoke(instance);
        }
    }
}