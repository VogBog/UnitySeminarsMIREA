using System;

namespace Game.GameFinisher
{
    public interface IGameFinisher
    {
        event Action Died; 
        
        void InvokeDiedToOwner(Player.Player player);
        void QuitFromGame();
    }
}