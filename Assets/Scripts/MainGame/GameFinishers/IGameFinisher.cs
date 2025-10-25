using System;

namespace MainGame.GameFinishers
{
    public interface IGameFinisher
    {
        event Action<Player.Player> PlayerWinned; 
        event Action Finished;

        bool CanFinishGame();
        void InvokePlayerWinned(Player.Player player);
        void InvokeFinished();
        void LoadScene(int sceneIndex);
    }
}