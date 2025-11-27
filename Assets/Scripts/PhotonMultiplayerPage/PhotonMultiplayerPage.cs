using LocalMultiplayerPage;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonMultiplayerPage
{
    public class PhotonMultiplayerPage : MonoBehaviourPunCallbacks
    {
        [SerializeField] private LocalMultiplayerPageView _view;
        [SerializeField] private PhotonMultiplayerLobby _lobby;

        private void Awake()
        {
            _view.Initialize();

            _view.CreateClicked += CreateRoom;
            _view.JoinClicked += JoinRoom;
            _view.QuitClicked += Quit;
            _lobby.Quitted += Quit;
            
            _view.HidePage();

            PhotonNetwork.AutomaticallySyncScene = true;

            var settings = PhotonNetwork.PhotonServerSettings.AppSettings;
            settings.FixedRegion = "ru";
            
            PhotonNetwork.ConnectUsingSettings(settings);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            
            _view.ShowPage();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            SceneManager.LoadScene(0);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            _view.ShowPage();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            _view.ShowPage();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            _view.ShowPage();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            _lobby.ShowLobby();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _lobby.ShowLobby();
        }

        private void CreateRoom(string roomName)
        {
            _view.HidePage();
            PhotonNetwork.JoinOrCreateRoom(
                roomName,
                new RoomOptions
                {
                    IsOpen = true,
                    IsVisible = true, 
                    MaxPlayers = 10
                },
                TypedLobby.Default);
        }

        private void JoinRoom(string roomName)
        {
            _view.HidePage();
            
            if (string.IsNullOrEmpty(roomName) || string.IsNullOrWhiteSpace(roomName))
            {
                PhotonNetwork.JoinRandomRoom();
                return;
            }

            PhotonNetwork.JoinRoom(roomName);
        }

        private void Quit()
        {
            _view.HidePage();
            PhotonNetwork.Disconnect();
        }
    }
}