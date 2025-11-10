using System;
using System.Net;
using System.Net.Sockets;
using Global;
using MainMenu;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LocalMultiplayerPage
{
    public class LocalMultiplayerPage : NetworkBehaviour
    {
        [Header("First Page")]
        [SerializeField] private GameObject _firstPage;
        [SerializeField] private TMP_InputField _ipField;
        [SerializeField] private Button _joinBtn;
        [SerializeField] private Button _createBtn;
        [SerializeField] private Button _backBtn;
        
        [Header("Connected page")]
        [SerializeField] private GameObject _connectedPage;
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _stopBtn;
        [SerializeField] private string _sceneName;

        private int _playersCount = 0;

        private void Start()
        {
            _joinBtn.onClick.AddListener(OnJoinBtnClicked);
            _createBtn.onClick.AddListener(OnCreateBtnClicked);
            _backBtn.onClick.AddListener(OnBackBtnClicked);
            _startBtn.onClick.AddListener(StartBtnClicked);
            _stopBtn.onClick.AddListener(StopBtnClicked);
            
            NetworkManager.Singleton.OnClientStopped += OnClientStopped;
            NetworkManager.Singleton.OnConnectionEvent += OnConnected;
            NetworkManager.Singleton.OnTransportFailure += OnTransportError;

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(GetLocalIPv4(), transport.ConnectionData.Port);
            _ipField.text = transport.ConnectionData.Address;
            
            CloseAll();
            _firstPage.SetActive(true);
            _startBtn.interactable = false;
        }

        public override void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientStopped -= OnClientStopped;
                NetworkManager.Singleton.OnConnectionEvent -= OnConnected;
                NetworkManager.Singleton.OnTransportFailure -= OnTransportError;
            }
            
            base.OnDestroy();
        }

        private void OnJoinBtnClicked()
        {
            _playersCount = 0;
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(_ipField.text, transport.ConnectionData.Port);

            CloseAll();
            _startBtn.interactable = false;
            NetworkManager.StartClient();
        }

        private void OnCreateBtnClicked()
        {
            _playersCount = 0;
            
            CloseAll();
            string ipAddress = _ipField.text;
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData(ipAddress, transport.ConnectionData.Port);
            NetworkManager.StartHost();
        }

        private void OnBackBtnClicked()
        {
            CloseAll();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene(0);
        }

        private void StartBtnClicked()
        {
            if (!IsServer)
                return;
            
            CloseAll();
            StaticParameters.PlayersCount = _playersCount;
            StaticParameters.GameType = GameTypes.LocalMultiplayer;
            NetworkManager.Singleton.SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }

        private void StopBtnClicked()
        {
            CloseAll();
            NetworkManager.Singleton.Shutdown();
        }

        private void OnTransportError()
        {
            CloseAll();
            _firstPage.SetActive(true);
        }

        private void OnClientStopped(bool isHost)
        {
            _firstPage.SetActive(true);
        }

        private void CloseAll()
        {
            _firstPage.SetActive(false);
            _connectedPage.SetActive(false);
        }

        private void OnConnected(NetworkManager manager, ConnectionEventData data)
        {
            if (data.EventType is ConnectionEvent.ClientDisconnected && IsServer)
            {
                OnPlayerDisconnected();
                return;
            }
            
            if (data.ClientId != manager.LocalClientId)
                return;

            if (data.EventType is ConnectionEvent.ClientConnected)
            {
                CloseAll();
                _connectedPage.SetActive(true);
                SetInfo();
                NewPlayerServerRpc();
            }
            else if(data.EventType is ConnectionEvent.ClientDisconnected)
            {
                CloseAll();
                _firstPage.SetActive(true);
            }
        }

        private void SetInfo()
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            _infoText.text = $"Connected: {transport.ConnectionData.Address}{Environment.NewLine}" +
                             $"Players: {_playersCount}";
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void NewPlayerServerRpc()
        {
            _playersCount++;
            SetPlayersCountClientRpc(_playersCount);
            _startBtn.interactable = _playersCount > 1;
        }

        private void OnPlayerDisconnected()
        {
            _playersCount--;
            _startBtn.interactable = _playersCount > 1;
            SetPlayersCountClientRpc(_playersCount);
        }

        [ClientRpc]
        private void SetPlayersCountClientRpc(int count)
        {
            _playersCount = count;
            StaticParameters.PlayersCount = count;
            StaticParameters.GameType = GameTypes.LocalMultiplayer;
            
            SetInfo();
        }
        
        public string GetLocalIPv4()
        {
            string localIP = "127.0.0.1";
        
            try
            {
                string hostName = Dns.GetHostName();
                Debug.Log($"Host name: {hostName}");
            
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ip = address.ToString();
                    
                        if (ip != "127.0.0.1")
                        {
                            localIP = ip;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error getting IP address: {e.Message}");
            }
        
            return localIP;
        }
    }
}