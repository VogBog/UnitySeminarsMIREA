using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonLobby
{
    [Serializable]
    public class PhotonLobbyFirstPageView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private Button _createBtn;
        [SerializeField] private Button _joinBtn;
        [SerializeField] private Button _quitBtn;

        public event Action CreateClicked;
        public event Action JoinClicked;
        public event Action QuitClicked;
        
        public void Initialize()
        {
            _createBtn.onClick.AddListener(() => CreateClicked?.Invoke());
            _joinBtn.onClick.AddListener(() => JoinClicked?.Invoke());
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
    }
}