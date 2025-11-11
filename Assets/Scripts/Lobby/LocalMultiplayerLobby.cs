using System;
using InputSystems;
using MainMenu;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class LocalMultiplayerLobby : NetworkBehaviour, ILobby
    {
        private Lobby _lobby;
        private PlayerInput _input;
        private int _playerIndex;
        private int _readyPlayersCount;
        private bool _companyStarted;
        private string _addNewPlayerMess;

        public bool IsAllPlayersActive => false;
        public NetworkTypes NetworkType => NetworkTypes.LocalMultiplayer;
        public LocalMultiplayerConnectionData ConnectionData { get; private set; }
        
        public event Action<PlayerData> PlayerChanged;
        public event Action<int> PlayerDisconnected;
        public event Action<PlayerData[], bool> CompanyStarted;
        
        public void OnInitializeLobby(Lobby lobby, TMP_Text titleText)
        {
            _lobby = lobby;
            _playerIndex = -1;
            _companyStarted = false;
            _input = new(1);
            _input.EndInteraction += OnInteracting;

            var mainMenu = FindFirstObjectByType<MainMenuPages>();
            ConnectionData = mainMenu.LocalMultiplayerConnectionData;

            titleText.text = $"Ip: {ConnectionData.Ip}";
            
            OnJoinedLobby();
        }

        private void OnInteracting(PlayerInput input)
        {
            if (_addNewPlayerMess != "" || _playerIndex < 0)
                return;
            
            NextElementalServerRpc(_playerIndex);
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
            var player = new PlayerData(false, index, null, _lobby.AllElements[0]);
            
            if (_addNewPlayerMess == mess)
            {
                _playerIndex = player.Index;
                player.IsActive = true;
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
        
        #region NextElemental

        [ServerRpc(RequireOwnership = false)]
        private void NextElementalServerRpc(int playerIndex)
        {
            var player = _lobby.GetPlayer(playerIndex);
            int index = 0;
            for (int i = 0; i < _lobby.AllElements.Length; i++)
            {
                if (_lobby.AllElements[i].Name == player.Data.Name)
                {
                    index = i;
                    break;
                }
            }
            
            index = (index + 1) % _lobby.AllElements.Length;
            NextElementalClientRpc(playerIndex, index);
        }

        [ClientRpc]
        private void NextElementalClientRpc(int playerIndex, int elementalIndex)
        {
            var player = _lobby.GetPlayer(playerIndex);
            var element = _lobby.AllElements[elementalIndex];
            player.Data = element;
            
            PlayerChanged?.Invoke(player);
        }
        #endregion

        public void OnLeaveLobby()
        {
            PlayerDisconnectedServerRpc(_playerIndex);
            NetworkManager.Singleton.Shutdown();
        }

        public void OnDisposeLobby()
        {
            _input.EndInteraction -= OnInteracting;
        }

        #region CompanyStarted
        public void InvokeCompanyStarted(PlayerData[] players)
        {
            var indexes = new int[players.Length];
            var elementals = new int[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                int elementIndex = 0;
                for (int j = 0; j < _lobby.AllElements.Length; j++)
                {
                    if (_lobby.AllElements[j].Name == players[i].Data.Name)
                    {
                        elementIndex = j;
                        break;
                    }
                }

                indexes[i] = players[i].Index;
                elementals[i] = elementIndex;
            }

            InvokeCompanyStartedServerRpc(indexes, elementals);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InvokeCompanyStartedServerRpc(int[] indexes, int[] elementals)
        {
            if (_companyStarted)
                return;
            _companyStarted = true;

            _readyPlayersCount = 0;
            InvokeCompanyStartedClientRpc(indexes, elementals);
        }

        [ClientRpc]
        private void InvokeCompanyStartedClientRpc(int[] indexes, int[] elementals)
        {
            var players = new PlayerData[Mathf.Min(indexes.Length, elementals.Length)];
            
            for (int i = 0; i < players.Length; i++)
            {
                var elemental = _lobby.AllElements[elementals[i]];
                var index = indexes[i];
                bool isActive = index == _playerIndex;

                players[i] = new PlayerData(isActive, index, null, elemental);
            }
            
            CompanyStarted?.Invoke(players, NetworkManager.Singleton.IsHost);
        }
        #endregion
        
        #region LoadScene
        public void LoadScene(int sceneIndex, bool isHost)
        {
            LoadSceneServerRpc(sceneIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void LoadSceneServerRpc(int sceneIndex)
        {
            _readyPlayersCount++;
            if (_readyPlayersCount < _lobby.GetAllPlayers().Length)
                return;

            string path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        #endregion
    }
}