using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.FruitsSpawner
{
    public class PhotonFruitsSpawner : MonoBehaviour, IFruitsSpawner
    {
        private Fruit _fruitPrefab;
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;

        public bool IsServer() => PhotonNetwork.IsMasterClient;

        public void SetPrefab(Fruit fruitPrefab) => _fruitPrefab = fruitPrefab;

        public void Instantiate(Vector3 position)
        {
            if (!IsServer())
                return;
            
            PhotonNetwork.Instantiate(
                _fruitPrefab.name + " Variant",
                position,
                Quaternion.identity);
        }

        public void Despawn(Fruit fruit)
        {
            if (!IsServer())
                return;
            
            PhotonNetwork.Destroy(fruit.gameObject);
        }
    }
}