using UnityEngine;
using UnityEngine.SceneManagement;

namespace Var2
{
    public class AsteroidMainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("AsteroidGame");
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quitted");
        }
    }
}
