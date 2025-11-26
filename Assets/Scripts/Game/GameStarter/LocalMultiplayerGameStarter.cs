using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Unity.Netcode;
using UnityEngine;

namespace Game.GameStarter
{
    public class LocalMultiplayerGameStarter : NetworkBehaviour, IGameStarter
    {
        private int _spawnedCount = 0;
        private List<ulong> _connectedPlayers = new();

        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.LocalMultiplayer;
        
        public event Action<int> TimerChanged;
        public event Action<Player.Player> Initialized; 

        bool IGameStarter.IsServer() => NetworkManager.IsServer;

        public IEnumerator InstantiatePlayer(
            Player.Player prefab, Vector3 position, Quaternion rotation)
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

            if (NetworkManager.LocalClientId == id)
            {
                Initialized?.Invoke(instance);
            }
            else
            {
                InitializeRpc(id);
            }

            yield return null;
        }

        public bool IsAllPlayersConnected()
        {
            var ids = NetworkManager.ConnectedClientsIds;
            return _connectedPlayers.Count == ids.Count;
        }

        public void InvokePlayerConnected()
        {
            InvokePlayerConnectedRpc(NetworkManager.LocalClientId);
        }

        [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
        private void InvokePlayerConnectedRpc(ulong id)
        {
            if(!_connectedPlayers.Contains(id))
                _connectedPlayers.Add(id);
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

        [Rpc(SendTo.Everyone)]
        private void InitializeRpc(ulong ownerId)
        {
            if (NetworkManager.LocalClientId == ownerId)
                StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            for (int attempt = 0; attempt < 1_000; attempt++)
            {
                var players = NetworkManager.SpawnManager.PlayerObjects;
                var player = players.FirstOrDefault(x => x.IsOwner);

                if (player?.TryGetComponent(out Player.Player result) ?? false)
                {
                    Initialized?.Invoke(result);
                    yield break;
                }

                yield return null;
            }
        }
    }
}