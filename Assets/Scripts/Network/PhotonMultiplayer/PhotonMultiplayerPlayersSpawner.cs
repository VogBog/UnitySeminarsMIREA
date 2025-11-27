using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.PhotonMultiplayer
{
    public class PhotonMultiplayerPlayersSpawner : MonoBehaviour, IPlayerSpawnerPolitics
    {
        private readonly List<(int, Action<Player>)> _waitingList = new();
        private readonly List<int> _spawnedActors = new();
        private string _prefabName;
        private Vector3[] _spawnPoints;

        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }
        
        public void SetPrefab(Player prefab)
        {
            _prefabName = prefab.name + " Variant";
        }

        public void SetSpawnPoints(Vector3[] positions)
        {
            _spawnPoints = positions;
        }

        public void Instantiate(Action<Player> onSpawn)
        {
            var players = PhotonNetwork.CurrentRoom.Players.Values;
            int actorNumber = -1;
            
            foreach (var player in players)
            {
                if (!_spawnedActors.Contains(player.ActorNumber))
                {
                    actorNumber = player.ActorNumber;
                    break;
                }
            }

            if (actorNumber == -1)
                return;
            
            _spawnedActors.Add(actorNumber);
            _waitingList.Add((actorNumber, onSpawn));
            
            var position = _spawnPoints[actorNumber];
            _photonView.RPC(nameof(InstantiateRpc), RpcTarget.All, _prefabName, actorNumber, position);
        }

        [PunRPC]
        private void InstantiateRpc(string prefabName, int actorNumber, Vector3 position)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
                return;

            var go = PhotonNetwork.Instantiate(
                prefabName,
                position,
                Quaternion.identity);
            
            var photonView = go.GetComponent<PhotonView>();
            int viewId = photonView.ViewID;
            _photonView.RPC(nameof(InstantiateCallback), RpcTarget.MasterClient, actorNumber, viewId);
        }

        [PunRPC]
        private void InstantiateCallback(int actorNumber, int viewId)
        {
            StartCoroutine(InstantiateCallbackRoutine(actorNumber, viewId));
        }

        private IEnumerator InstantiateCallbackRoutine(int actorNumber, int viewId)
        {
            for (int attempt = 0; attempt < 120; attempt++)
            {
                var photonView = PhotonView.Find(viewId);
                if (photonView == null)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                var player = photonView.GetComponent<Player>();
                if (player == null)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                for (int i = 0; i < _waitingList.Count; i++)
                {
                    if(_waitingList[i].Item1 != actorNumber)
                        continue;

                    var action = _waitingList[i].Item2;
                    _waitingList.RemoveAt(i);
                    action?.Invoke(player);

                    yield break;
                }
                
                yield return new WaitForSeconds(0.1f);
            }
            
            Debug.LogError($"Cannot find instantiated player with ViewID {viewId}");
        }

        public void DestroyPlayer(Player player)
        {
            PhotonNetwork.Destroy(player.gameObject);
        }

        public void LoadScene(int sceneIndex)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(sceneIndex);
        }
    }
}