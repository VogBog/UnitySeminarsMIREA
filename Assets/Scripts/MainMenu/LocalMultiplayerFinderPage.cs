using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    [Serializable]
    public class LocalMultiplayerFinderPage : IDisposable
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private Button _createBtn, _joinBtn, _quitBtn;
        [SerializeField] private TMP_InputField _ipInput;

        private bool _isHost;

        public event Action Quit;
        public event Action<LocalMultiplayerConnectionData> Started;

        public void Initialize()
        {
            _createBtn.onClick.AddListener(OnCreateBtnClicked);
            _joinBtn.onClick.AddListener(OnJoinBtnClicked);
            _quitBtn.onClick.AddListener(OnQuitBtnClicked);

            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        }

        public void Dispose()
        {
            NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
        }

        public void SetActive(bool active)
        {
            _parent.SetActive(active);
        }

        private void BeforeStartFinding(bool isHost)
        {
            UpdateIp();
            _isHost = isHost;
            SetActive(false);
        }

        private void OnCreateBtnClicked()
        {
            BeforeStartFinding(true);
            NetworkManager.Singleton.StartHost();
        }

        private void OnJoinBtnClicked()
        {
            BeforeStartFinding(false);
            NetworkManager.Singleton.StartClient();
        }

        private void OnQuitBtnClicked()
        {
            SetActive(false);
            Quit?.Invoke();
            Dispose();
        }

        public void UpdateIp()
        {
            string ip = _ipInput.text;
            if (string.IsNullOrEmpty(ip) || string.IsNullOrWhiteSpace(ip))
                return;
            
            var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
            if (transport == null)
            {
                Debug.LogError("Cannot connect to Unity Network Transport");
                return;
            }
            
            transport.SetConnectionData(
                ip,
                transport.ConnectionData.Port,
                transport.ConnectionData.ServerListenAddress);
        }

        public string GetIp()
        {
            var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
            if (transport == null)
            {
                Debug.LogError("Cannot connect to Unity Network Transport");
                return _ipInput.text;
            }

            return transport.ConnectionData.Address;
        }

        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData eventData)
        {
            SetActive(true);
            
            if (eventData.EventType is ConnectionEvent.ClientConnected)
            {
                SetActive(false);
                Started?.Invoke(new(_isHost, GetIp()));
                Dispose();
            }
        }
    }
}