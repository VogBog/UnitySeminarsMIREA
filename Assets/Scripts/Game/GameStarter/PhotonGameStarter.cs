using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.GameStarter
{
    public class PhotonGameStarter : MonoBehaviour, IGameStarter
    {
        private PhotonView _photonView;
        private bool _instantiatingInProcess = false;
        private readonly List<int> _players = new();
        private readonly List<int> _createdPlayers = new();
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;
        
        public event Action<int> TimerChanged;
        public event Action<Player.Player> Initialized;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public bool IsServer() => PhotonNetwork.IsMasterClient;

        public IEnumerator InstantiatePlayer(Player.Player prefab, Vector3 position, Quaternion rotation)
        {
            var players = PhotonNetwork.CurrentRoom.Players;
            int actorNumber = 0;
            
            foreach (var kvp in players)
            {
                if(_createdPlayers.Contains(kvp.Value.ActorNumber))
                    continue;
                actorNumber = kvp.Value.ActorNumber;
                break;
            }
            
            _createdPlayers.Add(actorNumber);
            _instantiatingInProcess = true;
            _photonView.RPC(nameof(InstantiatePlayerRpc), RpcTarget.All, actorNumber, prefab.name, position, rotation);
            
            yield return new WaitWhile(() => _instantiatingInProcess);
        }

        private void InstantiatePlayerRpc(int actorNumber, string prefabName, Vector3 position, Quaternion rotation)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
                return;

            var go = PhotonNetwork.Instantiate(prefabName, position, rotation);
            var player = go.GetComponent<Player.Player>();
            Initialized?.Invoke(player);
            
            _photonView.RPC(nameof(InstantiatePlayerCallback), RpcTarget.All);
        }

        private void InstantiatePlayerCallback()
        {
            _instantiatingInProcess = false;
        }

        public bool IsAllPlayersConnected()
        {
            return _players.Count == PhotonNetwork.CurrentRoom.PlayerCount;
        }

        public void InvokePlayerConnected()
        {
            _photonView.RPC(nameof(PlayerConnectedRpc), RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        private void PlayerConnectedRpc(int actorNumber)
        {
            if(!_players.Contains(actorNumber))
                _players.Add(actorNumber);
        }

        public void InvokeTimerChanged(int value)
        {
            TimerChanged?.Invoke(value);
            _photonView.RPC(nameof(TimerChangedRpc), RpcTarget.All, value);
        }

        private void TimerChangedRpc(int value)
        {
            TimerChanged?.Invoke(value);
        }
    }
}