using Extensions;
using Lobby;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuPages : MonoBehaviour
    {
        [SerializeField] private GameObject[] _pages;
        
        [Header("Network")]
        [SerializeField] private Button _networkButton;
        [SerializeField] private NetworkPage _networkPage;
        
        [FormerlySerializedAs("_localMultiplayerPage")]
        [Header("Local Multiplayer")]
        [SerializeField] private LocalMultiplayerAutoFinder _localMultiplayerAutoFinder;
        
        private Lobby.Lobby _lobby;

        private void Start()
        {
            _lobby = this.FindFirstObjectByTypeOrException<Lobby.Lobby>();
            _networkPage.Initialize(_networkButton);
            _networkPage.BackClicked += () => OpenPage(0);
            
            OpenPage(0);
        }

        public void CloseAll()
        {
            foreach (var page in _pages)
            {
                page.SetActive(false);
            }
        }

        public void OpenPage(int index)
        {
            CloseAll();
            _pages[index].SetActive(true);
        }

        public void OpenSplitScreenLobby()
        {
            OpenPage(1);
            _lobby.InitializeSplitScreenLobby();
        }

        public void OpenNetworkPage()
        {
            OpenPage(2);
        }

        public void OpenLocalMultiplayerPage()
        {
            OpenPage(1);
            _lobby.InitializeLocalMultiplayerLobby();
        }

        public void QuitFromLobby()
        {
            _lobby.LeaveLobby();
            OpenPage(0);
        }

        public void QuitClicked()
        {
            Application.Quit();
        }
    }
}