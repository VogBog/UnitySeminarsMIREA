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
        private Action<Player> _onSpawn;
        
        public void SetPrefab(Player prefab)
        {
            _prefab = prefab;
        }
        
        public void Instantiate(Vector3 pos, Quaternion rot, Transform parent, Action<Player> onSpawn)
        {
            ulong ownerId = NetworkManager.LocalClientId;
            _onSpawn = onSpawn;
            
            SpawnPlayerRpc(pos, rot, ownerId);
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void SpawnPlayerRpc(Vector3 pos, Quaternion rot, ulong ownerClientId)
        {
            StartCoroutine(SpawnPlayerRoutine(pos, rot, ownerClientId));
        }

        private IEnumerator SpawnPlayerRoutine(Vector3 pos, Quaternion rot, ulong ownerClientId)
        {
            while (_prefab == null)
                yield return null;
            
            var networkObject = _prefab.GetComponent<NetworkObject>();
            if(networkObject == null)
                throw new NullReferenceException("Player must have NetworkObject component");
            
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