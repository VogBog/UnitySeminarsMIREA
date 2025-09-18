using Extensions;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuPages : MonoBehaviour
    {
        [SerializeField] private GameObject[] _pages;
        
        private Lobby.Lobby _lobby;

        private void Start()
        {
            _lobby = this.FindFirstObjectByTypeOrException<Lobby.Lobby>();
            
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
            _lobby.InitializeLobbyHost();
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