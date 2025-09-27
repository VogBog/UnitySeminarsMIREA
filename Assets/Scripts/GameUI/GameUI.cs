using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            _playBtn.onClick.AddListener(OnPlayBtnClicked);
            _quitBtn.onClick.AddListener(OnQuitBtnClicked);
            _pauseMenu.SetActive(false);
        }

        private void OnPlayBtnClicked()
        {
            ClosePauseMenu();
        }

        private void OnQuitBtnClicked()
        {
            ClosePauseMenu();
            SceneManager.LoadScene(0);
        }

        public void ClosePauseMenu()
        {
            Time.timeScale = 1f;
            _pauseMenu.SetActive(false);
        }

        public void OpenPauseMenu()
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(_pauseMenu.activeSelf)
                    ClosePauseMenu();
                else
                    OpenPauseMenu();
            }
        }
    }
}