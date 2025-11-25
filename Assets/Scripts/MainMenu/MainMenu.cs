using Base;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _localMultiplayerBtn;
        [SerializeField] private Button _photonMultiplayerBtn;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            _localMultiplayerBtn.onClick.AddListener(OnLocalMultiplayerClicked);
            _photonMultiplayerBtn.onClick.AddListener(OnPhotonMultiplayerClicked);
            _quitBtn.onClick.AddListener(OnQuitClicked);
        }

        private void OnLocalMultiplayerClicked()
        {
            SceneManager.LoadScene((int)Scenes.LocalMultiplayerLobby);
        }

        private void OnPhotonMultiplayerClicked()
        {
            SceneManager.LoadScene((int)Scenes.PhotonMultiplayerLobby);
        }
        
        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}