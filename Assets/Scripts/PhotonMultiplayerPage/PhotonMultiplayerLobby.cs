using Global;
using MainMenu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PhotonMultiplayerPage
{
    public class PhotonMultiplayerLobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MultiplayerLobbyView _view;

        private bool _started = false;

        private void Awake()
        {
            _view.Initialize();

            _view.StartClicked += StartGame;
            _view.QuitClicked += Quit;
            
            _view.SetActive(false);
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

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _view.SetActive(true);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            int playersCount = PhotonNetwork.CurrentRoom?.PlayerCount ?? 0;
            int maxPlayers = PhotonNetwork.CurrentRoom?.MaxPlayers ?? 0;
            string roomName = PhotonNetwork.CurrentRoom?.Name ?? "Empty";

            StaticParameters.PlayersCount = playersCount;
            StaticParameters.GameType = GameTypes.Photon;
            
            _view.UpdateStatus(roomName, playersCount, maxPlayers, PhotonNetwork.IsMasterClient);
        }

        public void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient || _started)
                return;
            _started = true;
            _view.SetActive(false);
            
            PhotonNetwork.LoadLevel(1);
        }

        public void Quit() => PhotonNetwork.Disconnect();
    }
}