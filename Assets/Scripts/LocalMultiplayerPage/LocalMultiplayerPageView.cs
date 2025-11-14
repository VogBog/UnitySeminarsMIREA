using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayerPage
{
    [Serializable]
    public class LocalMultiplayerPageView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_InputField _ipField;
        [SerializeField] private Button _joinBtn;
        [SerializeField] private Button _createBtn;
        [SerializeField] private Button _quitBtn;

        public event Action<string> JoinClicked;
        public event Action<string> CreateClicked;
        public event Action QuitClicked;

        public void Initialize()
        {
            _joinBtn.onClick.AddListener(OnJoinBtnClicked);
            _createBtn.onClick.AddListener(OnCreateBtnClicked);
            _quitBtn.onClick.AddListener(OnQuitBtnClicked);
        }

        private void OnJoinBtnClicked()
        {
            _parent.SetActive(false);
            JoinClicked?.Invoke(_ipField.text);
        }

        private void OnCreateBtnClicked()
        {
            _parent.SetActive(false);
            CreateClicked?.Invoke(_ipField.text);
        }

        private void OnQuitBtnClicked()
        {
            _parent.SetActive(false);
            QuitClicked?.Invoke();
        }

        public void ShowPage()
        {
            _parent.SetActive(true);
        }
    }
}