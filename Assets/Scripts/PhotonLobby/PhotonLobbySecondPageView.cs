using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonLobby
{
    [Serializable]
    public class PhotonLobbySecondPageView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;

        public event Action StartClicked;
        public event Action QuitClicked;
        
        public void Initialize()
        {
            UpdateStatus(0, 0, false);
            
            _startBtn.onClick.AddListener(() => StartClicked?.Invoke());
            _quitBtn.onClick.AddListener(() => QuitClicked?.Invoke());
        }

        public void Show()
        {
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _parent.SetActive(active);
        }

        public void UpdateStatus(int playersCount, int maxPlayers, bool isHost)
        {
            _text.text = $"Players: {playersCount}/{maxPlayers}";
            _startBtn.interactable = isHost;
        }
    }
}