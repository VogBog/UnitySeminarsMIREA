using Unity.Netcode;
using UnityEngine;

namespace Game.FruitsSpawner
{
    public class LocalMultiplayerFruitsSpawner : NetworkBehaviour, IFruitsSpawner
    {
        private NetworkObject _prefab;

        bool IFruitsSpawner.IsServer() => NetworkManager.IsServer;

        public void SetPrefab(Fruit fruitPrefab)
        {
            _prefab = fruitPrefab.GetComponent<NetworkObject>();
        }

        public void Instantiate(Vector3 position)
        {
            NetworkManager.SpawnManager.InstantiateAndSpawn(
                _prefab,
                NetworkManager.LocalClientId,
                true,
                position: position);
        }

        public void Despawn(Fruit fruit)
        {
            fruit.GetComponent<NetworkObject>().Despawn();
        }
    }
}