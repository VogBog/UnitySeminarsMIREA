using System;
using System.Collections.Generic;
using Base;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayerLobby
{
    public class LocalMultiplayerLobbySecondPage : NetworkBehaviour
    {
        [SerializeField] private LocalMultiplayerLobbySecondPageView _view;
        
        private readonly List<ulong> _players = new();
        private bool _starting = false;
        
        public event Action<int> PlayersCountChanged;
        public event Action<bool> IsServerChanged; 

        private void Start()
        {
            _view.Initialize(this);
            
            _view.StartClicked += OnStartClicked;
            _view.QuitClicked += OnQuitClicked;
            
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        public override void OnDestroy()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
            }
            
            base.OnDestroy();
        }

        private void OnClientConnected(ulong id)
        {
            if (!IsServer) return;
            
            if(!_players.Contains(id))
                _players.Add(id);
            
            UpdatePlayersRpc(_players.ToArray());
        }
        
        private void OnClientDisconnected(ulong id)
        {
            if (!IsServer) return;
            
            if (_players.Contains(id))
                _players.Remove(id);
            
            UpdatePlayersRpc(_players.ToArray());
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void UpdatePlayersRpc(ulong[] players)
        {
            if (!IsServer)
            {
                _players.Clear();
                _players.AddRange(players);
            }
            
            PlayersCountChanged?.Invoke(_players.Count);
            IsServerChanged?.Invoke(IsServer);
        }

        private void OnStartClicked()
        {
            if (!IsServer || _starting) return;
            
            _starting = true;
            NetworkManager.SceneManager.LoadScene(nameof(Scenes.Game), LoadSceneMode.Single);
        }

        private void OnQuitClicked()
        {
            LocalMultiplayerLobby.Shutdown();
        }
    }
}