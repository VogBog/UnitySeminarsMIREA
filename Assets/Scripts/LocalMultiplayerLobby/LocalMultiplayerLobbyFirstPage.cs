using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayerLobby
{
    [Serializable]
    public class LocalMultiplayerLobbyFirstPage
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _hostBtn;
        [SerializeField] private Button _joinBtn;
        [SerializeField] private Button _quitBtn;

        public event Action<string> HostClicked;
        public event Action<string> JoinClicked;
        public event Action QuitClicked;

        public void Initialize()
        {
            _hostBtn.onClick.AddListener(OnHostClicked);
            _joinBtn.onClick.AddListener(OnJoinClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);
        }

        public void OpenPage()
        {
            _parent.gameObject.SetActive(true);
        }

        private void OnHostClicked()
        {
            _parent.gameObject.SetActive(false);
            HostClicked?.Invoke(GetIp());
        }

        private void OnJoinClicked()
        {
            _parent.gameObject.SetActive(false);
            JoinClicked?.Invoke(GetIp());
        }

        private void OnQuitClicked()
        {
            _parent.gameObject.SetActive(false);
            QuitClicked?.Invoke();
        }

        private string GetIp()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text))
                return LocalMultiplayerLobby.GetIp();
            return _inputField.text;
        }
    }
}