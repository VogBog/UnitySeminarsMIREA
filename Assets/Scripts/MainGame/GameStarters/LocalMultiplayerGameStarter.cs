using System;
using System.Collections;
using Data;
using MainMenu;
using Unity.Netcode;
using UnityEngine;

namespace MainGame.GameStarters
{
    public class LocalMultiplayerGameStarter : NetworkBehaviour, IGameStarter
    {
        private Player.Player _playerPrefab;
        private bool _spawned = false;

        public void SetPlayerPrefab(Player.Player playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }

        public void DestroyIfNotLocalMultiplayer()
        {
            if(StaticParameters.NetworkType is not NetworkTypes.LocalMultiplayer)
                Destroy(gameObject);
        }
        
        public IEnumerator CreatePlayerRoutine(GameStarter gameStarter, Vector3 spawnPoint, Action<Player.Player> playerCreated)
        {
            SetPlayerPrefab(gameStarter.PlayerPrefab);
            
            ulong id = NetworkManager.Singleton.LocalClientId;
            _spawned = false;
            CreatePlayerServerRpc(id, spawnPoint);

            while (!_spawned)
                yield return null;

            Player.Player player = null;
            while (player == null)
            {
                yield return new WaitForSeconds(1f);

                var players = FindObjectsByType<Player.Player>(
                    FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var i in players)
                {
                    if (i.TryGetComponent<NetworkObject>(out var networkObject) &&
                        networkObject.IsOwner)
                    {
                        player = i;
                        break;
                    }
                }
            }
            
            playerCreated?.Invoke(player);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreatePlayerServerRpc(ulong id, Vector3 spawnPoint)
        {
            StartCoroutine(CreatePlayerRoutine(id, spawnPoint));
        }

        private IEnumerator CreatePlayerRoutine(ulong id, Vector3 spawnPoint)
        {
            while (_playerPrefab == null)
                yield return null;

            var networkObject = _playerPrefab.GetComponent<NetworkObject>();
            if (networkObject == null)
            {
                Debug.LogError("Player prefab has not NetworkObject");
                yield break;
            }

            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkObject, id,
                destroyWithScene: true,
                isPlayerObject: true,
                position: spawnPoint);

            CallbackClientRpc(id);
        }

        [ClientRpc]
        private void CallbackClientRpc(ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)
                _spawned = true;
        }
    }
}