using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuPages : MonoBehaviour
    {
        [SerializeField] private GameObject[] _pages;
        
        [Header("Network")]
        [SerializeField] private Button _networkButton;
        [SerializeField] private NetworkPage _networkPage;

        [Header("Local Multiplayer")] 
        [SerializeField] private LocalMultiplayerFinderPage _localMultiplayerFinderPage;
        
        private Lobby.Lobby _lobby;
        
        public LocalMultiplayerConnectionData LocalMultiplayerConnectionData { get; private set; }

        private void Start()
        {
            _lobby = this.FindFirstObjectByTypeOrException<Lobby.Lobby>();
            _networkPage.Initialize(_networkButton);
            _networkPage.BackClicked += () => OpenPage(0);

            _localMultiplayerFinderPage.Quit += OpenMainMenu;
            _localMultiplayerFinderPage.Started += OpenLocalMultiplayerLobby;
            
            _localMultiplayerFinderPage.Initialize();
            
            OpenMainMenu();
        }

        public void CloseAll()
        {
            foreach (var page in _pages)
            {
                page.SetActive(false);
            }
            _localMultiplayerFinderPage.SetActive(false);
        }

        public void OpenPage(int index)
        {
            CloseAll();
            _pages[index].SetActive(true);
        }
        
        public void OpenMainMenu() => OpenPage(0);

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
            CloseAll();
            _localMultiplayerFinderPage.SetActive(true);
        }

        private void OpenLocalMultiplayerLobby(LocalMultiplayerConnectionData connectionData)
        {
            LocalMultiplayerConnectionData = connectionData;
            _lobby.InitializeLocalMultiplayerLobby();
            OpenPage(1);
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