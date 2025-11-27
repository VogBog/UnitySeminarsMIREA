using System;
using System.Collections.Generic;
using Global;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayerPage
{
    public class LocalMultiplayerLobby : NetworkBehaviour, ILobby
    {
        [SerializeField] private LocalMultiplayerLobbyView _view;

        private readonly List<ulong> _players = new();
        private bool _isStarting = false;

        public event Action Quitted;
        public event Action<int> PlayersCountChanged;

        private void Awake()
        {
            _view.Initialize(this, LocalMultiplayerPage.GetUnityTransport().ConnectionData.Address);

            _view.QuitClicked += Quit;
            _view.StartClicked += Start;

            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        public override void OnDestroy()
        {
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
            
            base.OnDestroy();
        }
        
        public string GetRoomName() => LocalMultiplayerPage.GetUnityTransport().ConnectionData.Address;

        private void OnClientConnected(ulong id)
        {
            if (!IsServer)
                return;
            
            if(!_players.Contains(id))
                _players.Add(id);

            UpdateClientsClientRpc(_players.Count, _players.ToArray());
        }

        private void OnClientDisconnected(ulong id)
        {
            if (!IsServer)
                return;

            if (_players.Contains(id))
                _players.Remove(id);
            
            UpdateClientsClientRpc(_players.Count, _players.ToArray());
        }

        [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
        private void UpdateClientsClientRpc(int count, ulong[] ids)
        {
            _players.Clear();
            _players.AddRange(ids);
            StaticParameters.PlayersCount = count;
            PlayersCountChanged?.Invoke(count);
        }

        private void Quit()
        {
            NetworkManager.Shutdown();
            Quitted?.Invoke();
        }

        private void Start()
        {
            if (!IsServer || _isStarting)
                return;
            _isStarting = true;

            StaticParameters.PlayersCount = _players.Count;
            NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}