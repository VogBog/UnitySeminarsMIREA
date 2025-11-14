using Base;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayerLobby
{
    public class LocalMultiplayerLobby : NetworkBehaviour
    {
        [SerializeField] private LocalMultiplayerLobbyFirstPage _firstPage;

        private void Start()
        {
            _firstPage.Initialize();
            _firstPage.OpenPage();
            
            _firstPage.HostClicked += OnHostClicked;
            _firstPage.JoinClicked += OnJoinClicked;
            _firstPage.QuitClicked += OnQuitClicked;

            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        }

        public override void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
            }
            
            base.OnDestroy();
        }

        private void OnConnectionEvent(NetworkManager manager, ConnectionEventData ev)
        {
            if (ev.EventType is ConnectionEvent.ClientDisconnected or ConnectionEvent.PeerDisconnected &&
                ev.ClientId == NetworkManager.LocalClientId)
            {
                _firstPage.OpenPage();
            }
        }

        private void OnHostClicked(string ip)
        {
            SetIp(ip);
            NetworkManager.StartHost();
        }

        private void OnJoinClicked(string ip)
        {
            SetIp(ip);
            NetworkManager.StartClient();
        }

        private void OnQuitClicked()
        {
            Shutdown();
        }

        public static UnityTransport GetUnityTransport()
            => NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;

        public static void SetIp(string ip)
        {
            var transport = GetUnityTransport();
            transport.SetConnectionData(
                ip, transport.ConnectionData.Port, transport.ConnectionData.ServerListenAddress);
        }

        public static string GetIp() => GetUnityTransport().ConnectionData.Address;

        public static void Shutdown()
        {
            var manager = NetworkManager.Singleton;
            manager.Shutdown();
            Destroy(manager.gameObject);
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}