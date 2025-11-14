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

        public event Action StartClicked;
        public event Action QuitClicked;
        
        public void Initialize(LocalMultiplayerLobby lobby, string ip)
        {
            UpdateText(ip, 1);
            
            _startBtn.onClick.AddListener(OnStartClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);

            lobby.PlayersCountChanged += OnPlayersCountChanged;
        }

        private void OnPlayersCountChanged(int count)
        {
            ShowPage();
            string ip = LocalMultiplayerPage.GetUnityTransport().ConnectionData.Address;
            UpdateText(ip, count);
        }

        public void ShowPage()
        {
            _parent.SetActive(true);
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