using System;
using System.Collections;
using Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.LocalMultiplayer
{
    public class LocalMultiplayerPlayersSpawner : NetworkBehaviour, IPlayerSpawnerPolitics
    {
        private Player _prefab;
        private Vector3[] _spawnPoints;
        private int _spawnPointIndex = 0;
        private Action<Player> _onSpawn;
        
        public void SetPrefab(Player prefab)
        {
            _prefab = prefab;
        }

        public void SetSpawnPoints(Vector3[] positions)
        {
            _spawnPoints = positions;
        }
        
        public void Instantiate(Action<Player> onSpawn)
        {
            ulong ownerId = NetworkManager.LocalClientId;
            _onSpawn = onSpawn;
            
            SpawnPlayerRpc(ownerId);
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void SpawnPlayerRpc(ulong ownerClientId)
        {
            StartCoroutine(SpawnPlayerRoutine(ownerClientId));
        }

        private IEnumerator SpawnPlayerRoutine(ulong ownerClientId)
        {
            while (_prefab == null || _spawnPoints == null)
                yield return null;
            
            var networkObject = _prefab.GetComponent<NetworkObject>();
            if(networkObject == null)
                throw new NullReferenceException("Player must have NetworkObject component");
            
            var pos = _spawnPoints[_spawnPointIndex];
            var rot = Quaternion.identity;
            _spawnPointIndex = (_spawnPointIndex + 1) % _spawnPoints.Length;
            
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkObject,
                ownerClientId: ownerClientId,
                destroyWithScene: true,
                isPlayerObject: true,
                position: pos,
                rotation: rot);
            
            PlayerSpawnedRpc(ownerClientId);
        }

        [Rpc(SendTo.Everyone)]
        private void PlayerSpawnedRpc(ulong ownerId)
        {
            if (NetworkManager.LocalClientId != ownerId)
                return;

            StartCoroutine(PlayerSpawnedRoutine(ownerId));
        }

        private IEnumerator PlayerSpawnedRoutine(ulong ownerClientId)
        {
            for(int attempt = 0; attempt < 10; attempt++)
            {
                foreach (var player in NetworkManager.SpawnManager.PlayerObjects)
                {
                    if (player.OwnerClientId == ownerClientId &&
                        player.TryGetComponent<Player>(out var result))
                    {
                        _onSpawn?.Invoke(result);
                        yield break;
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }

            throw new NullReferenceException("WTF");
        }

        public void DestroyPlayer(Player player)
        {
            player.LocalMultiplayerSync?.NetworkObject.Despawn();
        }

        public void LoadScene(int sceneIndex)
        {
            NetworkManager.Shutdown();
            SceneManager.LoadScene(sceneIndex);
        }
    }
}