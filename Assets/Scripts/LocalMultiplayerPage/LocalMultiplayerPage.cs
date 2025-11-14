using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayerPage
{
    public class LocalMultiplayerPage : MonoBehaviour
    {
        [SerializeField] private LocalMultiplayerPageView _view;
        [SerializeField] private LocalMultiplayerLobby _lobby;

        private void Awake()
        {
            _view.Initialize();

            _view.CreateClicked += Create;
            _view.JoinClicked += Join;
            _view.QuitClicked += Quit;

            _lobby.Quitted += Quit;

            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        }

        private void OnDestroy()
        {
            NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
        }

        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData ev)
        {
            if (ev.EventType is ConnectionEvent.ClientDisconnected or ConnectionEvent.PeerDisconnected &&
                ev.ClientId == NetworkManager.Singleton.LocalClientId)
            {
                _view.ShowPage();
            }
        }

        private void Join(string ip)
        {
            if (!string.IsNullOrEmpty(ip) && !string.IsNullOrWhiteSpace(ip))
            {
                SetIp(ip);
            }

            NetworkManager.Singleton.StartClient();
        }

        private void Create(string ip)
        {
            if(!string.IsNullOrEmpty(ip) && !string.IsNullOrWhiteSpace(ip))
                SetIp(ip);
            
            NetworkManager.Singleton.StartHost();
        }
        
        public static UnityTransport GetUnityTransport()
            => NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;

        public static void SetIp(string ip)
        {
            var transport = GetUnityTransport();
            transport.SetConnectionData(
                ip, transport.ConnectionData.Port, transport.ConnectionData.ServerListenAddress);
        }

        private void Quit()
        {
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}