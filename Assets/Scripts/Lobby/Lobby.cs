using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using InputSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private LobbyView _view;
        [SerializeField] private int _levelSceneIndex;

        private PlayerInput[] _inputs;
        private readonly Dictionary<PlayerInput, PlayerData> _activePlayers = new();
        private ElementalData[] _allElements;
        
        public event Action<PlayerData[]> ActivePlayersChanged; 
        
        public void InitializeLobbyHost()
        {
            _inputs = new[]
            {
                new PlayerInput(1), new PlayerInput(2),
                new PlayerInput(3), new PlayerInput(4)
            };

            foreach (var input in _inputs)
            {
                input.SomethingPressed += OnPlayerPressedSomething;
            }
            
            _allElements = GameResources.LoadAllElementalData();
            _view.PlayBtnClicked += StartCompany;
            
            _view.Initialize(this);
        }

        public void LeaveLobby()
        {
            DisposeLobby();
        }

        private void DisposeLobby()
        {
            foreach (var input in _activePlayers.Keys)
                input.Interacted -= ChangeElemental;
            
            foreach(var input in _inputs)
                input.Dispose();
            
            _inputs = null;
            _activePlayers.Clear();
        }

        private void OnPlayerPressedSomething(PlayerInput input)
        {
            int index = 0;
            for (int i = 0; i < _inputs.Length; i++)
            {
                if (_inputs[i] == input)
                {
                    index = i + 1;
                    break;
                }
            }

            if (!_activePlayers.TryAdd(input, new(index, _allElements[index % _allElements.Length])))
                return;
            
            ActivePlayersChanged?.Invoke(_activePlayers.Values.ToArray());
            input.Interacted += ChangeElemental;
        }

        private void ChangeElemental(PlayerInput input)
        {
            if (!_activePlayers.TryGetValue(input, out var data))
                return;

            int index = -1;
            for (int i = 0; i < _allElements.Length; i++)
            {
                if (_allElements[i] == data.Data)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                throw new Exception("Lobby::ChangeElemental() : Cannot find elemental data.");
            
            index = (index + 1) % _allElements.Length;
            data.Data = _allElements[index];
            _activePlayers[input] = data;
            
            ActivePlayersChanged?.Invoke(_activePlayers.Values.ToArray());
        }

        public void StartCompany()
        {
            var allPlayers = _activePlayers.Values.ToArray();
            StaticParameters.Players = allPlayers;
            
            DisposeLobby();
            
            SceneManager.LoadScene(_levelSceneIndex);
        }
    }
}