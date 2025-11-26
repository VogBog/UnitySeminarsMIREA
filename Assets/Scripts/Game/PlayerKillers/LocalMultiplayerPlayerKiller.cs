using Base;
using Unity.Netcode;

namespace Game.PlayerKillers
{
    public class LocalMultiplayerPlayerKiller : NetworkBehaviour, IPlayerKiller
    {
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.LocalMultiplayer;
        
        bool IPlayerKiller.IsServer() => NetworkManager.IsServer;
        
        public void InvokePlayerDied(Player.Player player)
        {
            var networkObject = player.GetComponent<NetworkObject>();
            if (networkObject.IsOwner)
            {
                player.Die();
            }
            else
            {
                ulong playerId = networkObject.OwnerClientId;
                InvokePlayerDiedClientRpc(playerId);
            }
        }

        [ClientRpc]
        private void InvokePlayerDiedClientRpc(ulong playerId)
        {
            if (NetworkManager.LocalClientId != playerId)
                return;

            var playerObjects = NetworkManager.SpawnManager.PlayerObjects;
            foreach (var networkObject in playerObjects)
            {
                if (networkObject.OwnerClientId == playerId)
                {
                    var player = networkObject.GetComponent<Player.Player>();
                    player.Die();
                    return;
                }
            }
        }
    }
}