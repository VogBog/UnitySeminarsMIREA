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
            StaticParameters.NetworkType = StaticParameters.NetworkTypes.Single;
            StaticParameters.PlayersCount = playersCount;
            SceneManager.LoadScene(1);
        }

        public void OpenLeaderboard()
        {
            _leaderboard.SetActive(true);
        }

        public void OpenLocalMultiplayerPage()
        {
            StaticParameters.NetworkType = StaticParameters.NetworkTypes.LocalMultiplayer;
            SceneManager.LoadScene(3);
        }

        public void OpenPhotonPage()
        {
            StaticParameters.NetworkType = StaticParameters.NetworkTypes.Photon;
            SceneManager.LoadScene(4);
        }

        public void CloseLeaderboard()
        {
            _leaderboard.SetActive(false);
        }
    }
}