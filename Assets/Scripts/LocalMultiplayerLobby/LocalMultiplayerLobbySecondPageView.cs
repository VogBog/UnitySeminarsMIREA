using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayerLobby
{
    [Serializable]
    public class LocalMultiplayerLobbySecondPageView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;

        public event Action StartClicked;
        public event Action QuitClicked;
        
        public void Initialize(LocalMultiplayerLobbySecondPage page)
        {
            _startBtn.onClick.AddListener(OnStartClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);
            
            page.PlayersCountChanged += OnPlayersCountChanged;
            page.IsServerChanged += OnPlayerServerChanged;
        }

        private void OnPlayersCountChanged(int count)
        {
            UpdateInfo(
                LocalMultiplayerLobby.GetIp(),
                count);
        }

        private void OnPlayerServerChanged(bool isServer)
        {
            _startBtn.interactable = isServer;
        }

        public void UpdateInfo(string ip, int playersCount)
        {
            _text.text = $"{ip}{Environment.NewLine}Players: {playersCount}";
        }

        private void OnStartClicked()
        {
            _parent.SetActive(false);
            StartClicked?.Invoke();
        }

        private void OnQuitClicked()
        {
            _parent.SetActive(false);
            QuitClicked?.Invoke();
        }
    }
}