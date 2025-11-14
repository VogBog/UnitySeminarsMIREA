using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderboard;
        
        public void Quit()
        {
            Application.Quit();
        }

        public void Play(int playersCount)
        {
            StaticParameters.PlayersCount = playersCount;
            SceneManager.LoadScene(1);
        }

        public void OpenLeaderboard()
        {
            _leaderboard.SetActive(true);
        }

        public void OpenLocalMultiplayerPage()
        {
            SceneManager.LoadScene(3);
        }

        public void CloseLeaderboard()
        {
            _leaderboard.SetActive(false);
        }
    }
}