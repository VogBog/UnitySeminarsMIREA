using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Game.PhotonMultiplayer
{
    public class PhotonCarsSpawner : MonoBehaviour, ICarsSpawner
    {
        private readonly List<int> _spawnedActorNumbers = new();
        private Action<CarMovement> _onSpawn;
        private bool _spawned = false;
        private bool _isMyCar = false;
        
        private PhotonView _photonView;
        
        public event Action<CarMovement> MustInitialize;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public bool CanSpawnCars() => PhotonNetwork.IsMasterClient;

        public bool AddCameraAndMovement() => _isMyCar;

        public IEnumerator Instantiate(CarMovement prefab, Vector3 position, Quaternion rotation, Action<CarMovement> onSpawn)
        {
            _spawned = false;
            
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if(_spawnedActorNumbers.Contains(player.ActorNumber))
                    continue;
                
                _spawnedActorNumbers.Add(player.ActorNumber);

                _isMyCar = player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
                _spawned = false;
                _onSpawn = car =>
                {
                    _spawned = true;
                    onSpawn?.Invoke(car);
                };
                
                _photonView.RPC(
                    nameof(InstantiateRpc), 
                    RpcTarget.All, 
                    prefab.name + " Variant",
                    position,
                    rotation,
                    player.ActorNumber);
                
                yield return new WaitUntil(() => _spawned);

                break;
            }
        }

        [PunRPC]
        private void InstantiateRpc(
            string prefabName,
            Vector3 position,
            Quaternion rotation,
            int actorNumber)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
                return;

            var go = PhotonNetwork.Instantiate(prefabName, position, rotation);
            var photonView = go.GetComponent<PhotonView>();
            int viewId = photonView.ViewID;
            _photonView.RPC(nameof(InstantiateCallback), RpcTarget.MasterClient, viewId);
            
            var car = go.GetComponent<CarMovement>();
            if(car != null)
                MustInitialize?.Invoke(car);
        }

        [PunRPC]
        private void InstantiateCallback(int viewId)
        {
            StartCoroutine(InstantiateCallbackRoutine(viewId));
        }

        private IEnumerator InstantiateCallbackRoutine(int viewId)
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                var photonView = PhotonView.Find(viewId);
                if (photonView == null)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                var car = photonView.GetComponent<CarMovement>();
                if (car == null)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                
                _onSpawn?.Invoke(car);
                yield break;
            }
            
            Debug.LogError($"Cannot find instantiated object with view id {viewId}");
        }
    }
}