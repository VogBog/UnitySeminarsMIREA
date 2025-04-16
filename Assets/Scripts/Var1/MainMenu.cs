using UnityEngine;
using UnityEngine.SceneManagement;

namespace Var1
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Scenes/Test 1/Game");
        }

        public void Quit()
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}