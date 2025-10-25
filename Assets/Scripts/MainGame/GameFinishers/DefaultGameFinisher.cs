using System;
using UnityEngine.SceneManagement;

namespace MainGame.GameFinishers
{
    public class DefaultGameFinisher : IGameFinisher
    {
        public event Action<Player.Player> PlayerWinned;
        public event Action Finished;

        public bool CanFinishGame() => true;

        public void InvokePlayerWinned(Player.Player player) => PlayerWinned?.Invoke(player);

        public void InvokeFinished() => Finished?.Invoke();

        public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
    }
}