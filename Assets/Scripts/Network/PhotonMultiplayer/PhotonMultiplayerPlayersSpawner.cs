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
        private string _prefabName;
        private Vector3[] _spawnPoints;
        
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
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            
            var position = _spawnPoints[actorNumber];

            var go = PhotonNetwork.Instantiate(
                _prefabName,
                position,
                Quaternion.identity);
            
            var player = go.GetComponent<Player>();
            onSpawn.Invoke(player);
        }

        public void DestroyPlayer(Player player)
        {
            if (player.TryGetComponent(out PhotonView photonView) &&
                photonView.IsMine)
            {
                PhotonNetwork.Destroy(player.gameObject);
            }
        }

        public void LoadScene(int sceneIndex)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(sceneIndex);
        }
    }
}