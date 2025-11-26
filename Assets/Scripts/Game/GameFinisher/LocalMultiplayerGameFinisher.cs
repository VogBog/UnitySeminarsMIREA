using System;
using Base;
using Unity.Netcode;

namespace Game.GameFinisher
{
    public class LocalMultiplayerGameFinisher : NetworkBehaviour, IGameFinisher
    {
        public event Action Died;
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.LocalMultiplayer;

        bool IGameFinisher.IsServer() => NetworkManager.IsServer;
        
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

        public void QuitFromGame()
        {
            NetworkManager.Shutdown();
            Destroy(NetworkManager.gameObject);
        }
    }
}