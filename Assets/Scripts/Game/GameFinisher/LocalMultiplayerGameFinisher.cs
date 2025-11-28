using System;
using Base;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Game.GameFinisher
{
    public class LocalMultiplayerGameFinisher : NetworkBehaviour, IGameFinisher
    {
        public event Action Died;
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.LocalMultiplayer;

        bool IGameFinisher.IsServer() => NetworkManager.IsServer;

        private void Awake()
        {
            NetworkManager.OnClientStopped += OnClientStopped;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (NetworkManager != null)
            {
                NetworkManager.OnClientStopped -= OnClientStopped;
            }
        }

        public void InvokeDiedToOwner(Player.Player player)
        {
            var networkObject = player.GetComponent<NetworkObject>();
            ulong id = networkObject.OwnerClientId;
            DiedClientRpc(id);
        }

        [ClientRpc]
        private void DiedClientRpc(ulong clientId)
        {
            if(NetworkManager.LocalClientId == clientId)
                Died?.Invoke();
        }

        private void OnClientStopped(bool isHost)
        {
            QuitFromGame();
            SceneManager.LoadScene(0);
        }

        public void QuitFromGame()
        {
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);
        }

        public void ServerQuitFromGame() => QuitFromGame();
    }
}