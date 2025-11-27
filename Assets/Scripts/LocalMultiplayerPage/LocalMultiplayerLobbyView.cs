using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayerPage
{
    [Serializable]
    public class LocalMultiplayerLobbyView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;

        private ILobby _lobby;

        public event Action StartClicked;
        public event Action QuitClicked;
        
        public void Initialize(ILobby lobby, string ip)
        {
            UpdateText(ip, 1);
            
            _startBtn.onClick.AddListener(OnStartClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);
            
            _lobby = lobby;
            lobby.PlayersCountChanged += OnPlayersCountChanged;
        }

        private void OnPlayersCountChanged(int count)
        {
            ShowPage();
            string ip = _lobby.GetRoomName();
            UpdateText(ip, count);
        }

        public void ShowPage()
        {
            _parent.SetActive(true);
        }

        public void HidePage()
        {
            _parent.SetActive(false);
        }

        public void UpdateText(string ip, int playersCount)
        {
            _text.text = $"{ip}{Environment.NewLine}{playersCount}";
        }

        private void OnStartClicked()
        {
            StartClicked?.Invoke();
        }

        private void OnQuitClicked()
        {
            QuitClicked?.Invoke();
        }
    }
}