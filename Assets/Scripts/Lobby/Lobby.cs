using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private LobbyView _view;
        [SerializeField] private int _levelSceneIndex;
        [SerializeField] private SplitScreenLobby _splitScreenLobby;
        [SerializeField] private LocalMultiplayerLobby _localMultiplayerLobby;

        public ElementalData[] AllElements { get; private set; }
        private ILobby _lobby;
        private readonly Dictionary<int, PlayerData> _activePlayers = new();
        
        public event Action<PlayerData[], bool> ActivePlayersChanged;

        public void InitializeSplitScreenLobby() => InitializeLobby(_splitScreenLobby);
        public void InitializeLocalMultiplayerLobby() => InitializeLobby(_localMultiplayerLobby);

        public void InitializeLobby(ILobby lobby)
        {
            AllElements = GameResources.LoadAllElementalData();
            _view.PlayBtnClicked += StartCompany;
            
            _view.Initialize(this);
            
            _lobby = lobby;
            _lobby.PlayerChanged += OnPlayerChanged;
            _lobby.PlayerDisconnected += OnPlayerDisconnected;
            _lobby.CompanyStarted += StartCompanyLocal;
            
            _lobby.OnInitializeLobby(this);

            StaticParameters.NetworkType = lobby.NetworkType;
        }

        private void OnPlayerChanged(PlayerData player)
        {
            if (!_activePlayers.TryAdd(player.Index, player))
            {
                _activePlayers[player.Index] = player;
            }
            
            ActivePlayersChanged?.Invoke(_activePlayers.Values.ToArray(), _lobby.IsAllPlayersActive);
        }

        private void OnPlayerDisconnected(int index)
        {
            _activePlayers.Remove(index);
            ActivePlayersChanged?.Invoke(_activePlayers.Values.ToArray(), _lobby.IsAllPlayersActive);
        }

        public void LeaveLobby()
        {
            _lobby.OnLeaveLobby();
            
            DisposeLobby();
        }

        private void DisposeLobby()
        {
            _lobby.OnDisposeLobby();
            
            _lobby.PlayerChanged -= OnPlayerChanged;
            _lobby.PlayerDisconnected -= OnPlayerDisconnected;
            _lobby.CompanyStarted -= StartCompanyLocal;
            _lobby = null;
        }

        public void StartCompany()
        {
            var allPlayers = _activePlayers.Values.ToArray();
            _lobby.InvokeCompanyStarted(allPlayers);
        }
        
        public PlayerData[] GetAllPlayers() => _activePlayers.Values.ToArray();

        public PlayerData GetPlayer(int index) => _activePlayers.GetValueOrDefault(index);

        private void StartCompanyLocal(PlayerData[] players, bool isHost)
        {
            StaticParameters.Players = players;
            StaticParameters.GameType = GameType.None;
            if (players.Length == 2) StaticParameters.GameType = GameType.P1Vs1;
            else if (players.Length == 4) StaticParameters.GameType = GameType.P1Vs3;
            
            var lobby = _lobby;
            
            DisposeLobby();
            
            lobby.LoadScene(_levelSceneIndex, isHost);
        }
    }
}