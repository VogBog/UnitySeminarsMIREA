using System;
using System.Collections.Generic;
using Data;
using UnityEngine.InputSystem;
using PlayerInput = InputSystems.PlayerInput;

namespace Lobby
{
    [Serializable]
    public class SplitScreenLobby : ILobby
    {
        private PlayerInput[] _inputs;
        private readonly Dictionary<PlayerInput, PlayerData> _activePlayers = new();
        private Lobby _lobby;
        
        public ElementalData[] AllElements => _lobby.AllElements;
        
        public event Action<PlayerData> PlayerChanged;
        public event Action<int> PlayerDisconnected;
        public event Action<PlayerData[], bool> CompanyStarted; 
        
        public void OnInitializeLobby(Lobby lobby)
        {
            _lobby = lobby;
            
            _inputs = new[]
            {
                new PlayerInput(1), new PlayerInput(2),
                new PlayerInput(3), new PlayerInput(4)
            };

            foreach (var input in _inputs)
            {
                input.SomethingPressed += OnPlayerPressedSomething;
            }
        }

        public void OnLeaveLobby()
        {
            
        }

        public void OnDisposeLobby()
        {
            foreach (var input in _activePlayers.Keys)
                input.Interacted -= ChangeElemental;

            foreach (var input in _inputs)
                input.Dispose();
            
            _inputs = null;
            _activePlayers.Clear();
        }

        public void InvokeCompanyStarted(PlayerData[] players)
        {
            CompanyStarted?.Invoke(players, true);
        }
        
        private void OnPlayerPressedSomething(PlayerInput input, InputDevice device)
        {
            int index = 0;
            for (int i = 0; i < _inputs.Length; i++)
            {
                if (_inputs[i].Device == device && device.description.deviceClass != "Keyboard")
                    return;
                
                if (_inputs[i] == input)
                {
                    index = i + 1;
                }
            }

            if (!_activePlayers.TryAdd(input, new(index, device, AllElements[index % AllElements.Length])))
                return;
            
            input.SomethingPressed -= OnPlayerPressedSomething;
            input.Device = device;
            
            PlayerChanged?.Invoke(_activePlayers[input]);
            input.Interacted += ChangeElemental;
        }
        
        private void ChangeElemental(PlayerInput input)
        {
            if (!_activePlayers.TryGetValue(input, out var data))
                return;

            int index = -1;
            for (int i = 0; i < AllElements.Length; i++)
            {
                if (AllElements[i] == data.Data)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                throw new Exception("Lobby::ChangeElemental() : Cannot find elemental data.");
            
            index = (index + 1) % AllElements.Length;
            data.Data = AllElements[index];
            _activePlayers[input] = data;
            
            PlayerChanged?.Invoke(_activePlayers[input]);
        }
    }
}