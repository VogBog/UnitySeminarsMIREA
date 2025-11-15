using Unity.Netcode;

namespace Game.PlayerKillers
{
    public class LocalMultiplayerPlayerKiller : NetworkBehaviour, IPlayerKiller
    {
        bool IPlayerKiller.IsServer() => NetworkManager.IsServer;
    }
}