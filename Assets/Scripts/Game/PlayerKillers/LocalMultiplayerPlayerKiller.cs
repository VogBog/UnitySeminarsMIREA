using Base;
using Unity.Netcode;

namespace Game.PlayerKillers
{
    public class LocalMultiplayerPlayerKiller : NetworkBehaviour, IPlayerKiller
    {
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.LocalMultiplayer;
        
        bool IPlayerKiller.IsServer() => NetworkManager.IsServer;
    }
}