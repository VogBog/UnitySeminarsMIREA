using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Var1
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image menu;

        private ulong score;
        private bool inMenu = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !inMenu)
            {
                score++;
                text.text = score.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                inMenu = !inMenu;
                menu.gameObject.SetActive(inMenu);
            }
        }

        public void Quit()
        {
            SceneManager.LoadScene("Scenes/Test 1/MainMenu");
        }
    }
}