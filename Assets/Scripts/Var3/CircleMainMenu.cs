using UnityEngine;
using UnityEngine.SceneManagement;

namespace Var3
{
    public class CircleMainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("CircleGame");
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("QUIT");
        }
    }
}