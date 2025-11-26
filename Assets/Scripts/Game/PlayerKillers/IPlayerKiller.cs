using MultiNetwork;

namespace Game.PlayerKillers
{
    public interface IPlayerKiller : INetworkTypeRequirer
    {
        bool IsServer();
        void InvokePlayerDied(Player.Player player);
    }
}