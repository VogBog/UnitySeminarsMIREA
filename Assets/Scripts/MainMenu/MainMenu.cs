using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
        }

        public void Play(int playersCount)
        {
            StaticParameters.PlayersCount = playersCount;
            StaticParameters.GameType = playersCount == 1 ? GameTypes.Single : GameTypes.SplitScreen;
            
            int scene = GetSceneIndexByPlayersCount(playersCount);
            SceneManager.LoadScene(scene);
        }

        public void OpenLocalMultiplayerScene()
        {
            StaticParameters.GameType = GameTypes.LocalMultiplayer;
            SceneManager.LoadScene(4);
        }

        public int GetSceneIndexByPlayersCount(int playersCount) =>
            playersCount switch
            {
                1 => 2,
                _ => 1
            };
    }
}