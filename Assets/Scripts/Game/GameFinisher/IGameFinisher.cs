using System;
using MultiNetwork;

namespace Game.GameFinisher
{
    public interface IGameFinisher : INetworkTypeRequirer
    {
        event Action Died;

        bool IsServer();
        void InvokeDiedToOwner(Player.Player player);
        void QuitFromGame();
    }
}