using Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonLobby
{
    public class PhotonLobbySecondPage : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonLobbySecondPageView _view;

        private bool _startingGame = false;

        private void Start()
        {
            _view.Initialize();

            _view.StartClicked += StartGame;
            _view.QuitClicked += LeaveGame;
        }

        public void Activate()
        {
            UpdateStatus();
            _view.Show();
        }

        public void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient || _startingGame)
                return;
            
            _view.Hide();
            _startingGame = true;

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(nameof(Scenes.Game));
        }

        public void LeaveGame()
        {
            _view.Hide();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            UpdateStatus();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            bool isHost = PhotonNetwork.IsMasterClient;
            int playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            
            _view.UpdateStatus(playersCount, maxPlayers, isHost);
            StaticParameters.PlayersCount = playersCount;
        }
    }
}