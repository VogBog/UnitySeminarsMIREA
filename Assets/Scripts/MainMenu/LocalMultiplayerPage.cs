using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace MainMenu
{
    [Serializable]
    public class LocalMultiplayerPage
    {
        public void Initialize()
        {
            
        }

        [SerializeField] private int _gamePort = 7777;
        [SerializeField] private int _batchSize = 20;
        [SerializeField] private int _connectionTimeoutMs = 100;
        
        private CancellationTokenSource _cancellationTokenSource;

        public async void StartLocalMultiplayer()
        {
            try
            {
                Debug.Log("Starting network discovery...");
                
                string hostIP = await FindHostInLocalNetworkAsync();
                
                if (!string.IsNullOrEmpty(hostIP))
                {
                    Debug.Log($"Found host! Connecting to: {hostIP}");
                    ConnectToHost(hostIP);
                }
                else
                {
                    Debug.Log("No host found, starting as host");
                    BecomeHost();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                BecomeHost();
            }
        }
        
        private async Task<string> FindHostInLocalNetworkAsync()
        {
            try
            {
                string localIP = GetLocalIP();
                string networkPrefix = localIP.Substring(0, localIP.LastIndexOf('.') + 1);
                
                _cancellationTokenSource = new CancellationTokenSource();
                
                Debug.Log($"Searching for host in network: {networkPrefix}*");
                
                for (int batchStart = 1; batchStart < 255; batchStart += _batchSize)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        return null;
                        
                    var batchTasks = new List<Task<string>>();
                    int batchEnd = Math.Min(batchStart + _batchSize, 255);
                    
                    for (int i = batchStart; i < batchEnd; i++)
                    {
                        string testIP = networkPrefix + i;
                        batchTasks.Add(CheckHostAsync(testIP, _cancellationTokenSource.Token));
                    }
                    
                    var batchResults = await Task.WhenAll(batchTasks);
                    
                    var foundHost = batchResults.FirstOrDefault(result => !string.IsNullOrEmpty(result));
                    if (foundHost != null)
                    {
                        _cancellationTokenSource.Cancel();
                        return foundHost;
                    }
                    
                    Debug.Log($"Checked batch {batchStart}-{batchEnd-1}, no host found");
                }
                
                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
        
        private async Task<string> CheckHostAsync(string ip, CancellationToken token)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(ip, _gamePort);
                    var timeoutTask = Task.Delay(_connectionTimeoutMs, token);
                    
                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                    
                    if (completedTask == connectTask && connectTask.IsCompletedSuccessfully)
                    {
                        return ip; // Успешное подключение!
                    }
                    
                    token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            return null;
        }
        
        private string GetLocalIP()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            return "127.0.0.1";
        }
        
        private void ConnectToHost(string ip)
        {
            try
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    transport.SetConnectionData(ip, (ushort)_gamePort);
                }
                
                if (!NetworkManager.Singleton.StartClient())
                {
                    Debug.LogError("Failed to connect as client, becoming host");
                    BecomeHost();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                BecomeHost();
            }
        }
        
        private void BecomeHost()
        {
            try
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    transport.SetConnectionData("0.0.0.0", (ushort)_gamePort);
                }
                
                NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
                
                if (NetworkManager.Singleton.StartHost())
                {
                    Debug.Log("Successfully started as host");
                }
                else
                {
                    Debug.LogError("Failed to start host");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, 
            NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            Debug.Log($"Client {request.ClientNetworkId} approved");
        }
        
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
        }
    }
}