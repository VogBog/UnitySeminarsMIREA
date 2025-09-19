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
            SceneManager.LoadScene(1);
        }
    }
}