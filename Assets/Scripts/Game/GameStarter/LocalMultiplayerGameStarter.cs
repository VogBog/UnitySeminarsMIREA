using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Game.GameStarter
{
    public class LocalMultiplayerGameStarter : NetworkBehaviour, IGameStarter
    {
        private int _spawnedCount = 0;
        
        public event Action<int> TimerChanged;

        bool IGameStarter.IsServer() => NetworkManager.IsServer;

        public IEnumerator InstantiatePlayer(
            Player.Player prefab, Vector3 position, Quaternion rotation, Action<Player.Player> onSpawn)
        {
            if (!prefab.TryGetComponent(out NetworkObject networkObject))
            {
                Debug.LogError("Cannot spawn player. It has no NetworkObject attached.");
                yield break;
            }

            var players = NetworkManager.ConnectedClientsIds;
            ulong id = players[_spawnedCount];
            _spawnedCount++;

            var instance = NetworkManager.SpawnManager.InstantiateAndSpawn(
                    networkObject,
                    id,
                    true,
                    true,
                    position: position,
                    rotation: rotation)
                .GetComponent<Player.Player>();
            
            onSpawn?.Invoke(instance);

            yield return null;
        }

        public void InvokeTimerChanged(int value)
        {
            TimerChangedRpc(value);
        }

        [Rpc(SendTo.Everyone)]
        private void TimerChangedRpc(int value)
        {
            TimerChanged?.Invoke(value);
        }
    }
}