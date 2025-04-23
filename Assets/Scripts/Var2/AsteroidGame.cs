using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Var2
{
    public class AsteroidGame : MonoBehaviour
    {
        [SerializeField] private Image _menu;
        
        private bool _menuOpened = false;
        
        public void QuitGame()
        {
            SceneManager.LoadScene("AsteroidMainMenu");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menuOpened = !_menuOpened;
                _menu.gameObject.SetActive(_menuOpened);
                Time.timeScale = _menuOpened ? 0 : 1;
            }
        }
    }
}
