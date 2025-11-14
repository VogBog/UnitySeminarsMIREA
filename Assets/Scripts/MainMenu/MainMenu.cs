using Base;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _localMultiplayerBtn;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            _localMultiplayerBtn.onClick.AddListener(OnLocalMultiplayerClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);
        }

        private void OnLocalMultiplayerClicked()
        {
            SceneManager.LoadScene((int)Scenes.LocalMultiplayerLobby);
        }
        
        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}