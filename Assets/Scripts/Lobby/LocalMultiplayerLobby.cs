using System;
using MainMenu;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class LocalMultiplayerLobby : NetworkBehaviour, ILobby
    {
        [SerializeField] private LocalMultiplayerAutoFinder _finder;

        private Lobby _lobby;
        private int _playerIndex;
        private string _addNewPlayerMess;
        
        public event Action<PlayerData> PlayerChanged;
        public event Action<int> PlayerDisconnected;
        public event Action<PlayerData[], bool> CompanyStarted;
        
        public void OnInitializeLobby(Lobby lobby)
        {
            _lobby = lobby;
            _playerIndex = -1;
            _finder.Finished += OnJoinedLobby;
            _finder.StartLocalMultiplayer();
        }

        private void OnJoinedLobby()
        {
            _addNewPlayerMess = "";
            int maxCount = Random.Range(4, 8);
            for (int i = 0; i < maxCount; i++)
            {
                char c = (char)Random.Range('a', 'Z');
                _addNewPlayerMess += c;
            }
            
            NewPlayerJoinedServerRpc(_addNewPlayerMess);
        }

        #region NewPlayerJoined
        [ServerRpc(RequireOwnership = false)]
        private void NewPlayerJoinedServerRpc(string mess)
        {
            var allPlayers = _lobby.GetAllPlayers();
            int index = 0;
            foreach (var player in allPlayers)
            {
                if (player.Index == index)
                    index++;
            }

            AddNewPlayerClientRpc(index, mess);
        }

        [ClientRpc]
        private void AddNewPlayerClientRpc(int index, string mess)
        {
            var player = new PlayerData(index, null, _lobby.AllElements[0]);
            
            if (_addNewPlayerMess == mess)
            {
                _playerIndex = player.Index;
                _addNewPlayerMess = "";
            }
            
            PlayerChanged?.Invoke(player);
        }
        #endregion

        #region PlayerDisconnected
        [ServerRpc(RequireOwnership = false)]
        private void PlayerDisconnectedServerRpc(int index)
        {
            PlayerDisconnectedClientRpc(index);
        }

        [ClientRpc]
        private void PlayerDisconnectedClientRpc(int index)
        {
            PlayerDisconnected?.Invoke(index);
        }
        #endregion

        public void OnLeaveLobby()
        {
            PlayerDisconnectedServerRpc(_playerIndex);
            NetworkManager.Singleton.Shutdown();
        }

        public void OnDisposeLobby()
        {
            _finder.Finished -= OnJoinedLobby;
            _finder.OnDestroy();
        }

        public void InvokeCompanyStarted(PlayerData[] players)
        {
            throw new NotImplementedException();
        }
    }
}