using System;

namespace Game.GameFinisher
{
    public interface IGameFinisher
    {
        event Action Died;

        bool IsServer();
        void InvokeDiedToOwner(Player.Player player);
        void QuitFromGame();
    }
}