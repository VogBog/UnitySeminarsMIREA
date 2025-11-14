using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayerPage
{
    public class LocalMultiplayerLobby : NetworkBehaviour
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
        }

        private void OnClientConnected(ulong id)
        {
            if (!IsServer)
                return;
            
            if(!_players.Contains(id))
                _players.Add(id);

            OnClientConnectedClientRpc(_players.Count, _players.ToArray());
        }

        [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
        private void OnClientConnectedClientRpc(int count, ulong[] ids)
        {
            _players.Clear();
            _players.AddRange(ids);
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

            NetworkManager.SceneManager.LoadScene("LMLevel", LoadSceneMode.Single);
        }
    }
}