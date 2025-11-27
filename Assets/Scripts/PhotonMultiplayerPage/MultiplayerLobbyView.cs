using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonMultiplayerPage
{
    [Serializable]
    public class MultiplayerLobbyView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;
        
        public event Action StartClicked;
        public event Action QuitClicked;
        
        public void Initialize()
        {
            _startBtn.onClick.AddListener(() => StartClicked?.Invoke());
            _quitBtn.onClick.AddListener(() => QuitClicked?.Invoke());
        }

        public void SetActive(bool active)
        {
            _parent.SetActive(active);
        }

        public void UpdateStatus(string roomName, int playersCount, int maxPlayers, bool isHost)
        {
            _text.text = $"{roomName}{Environment.NewLine}" +
                         $"Players: {playersCount}/{maxPlayers}";
            _startBtn.interactable = isHost;
        }
    }
}