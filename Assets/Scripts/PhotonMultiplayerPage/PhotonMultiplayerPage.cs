using Global;
using MainMenu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonMultiplayerPage
{
    public class PhotonMultiplayerPage : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MultiplayerPageView _view;

        private void Awake()
        {
            _view.Initialize();

            _view.CreateClicked += CreateRoom;
            _view.JoinClicked += JoinRoom;
            _view.QuitClicked += Quit;
            
            _view.SetActive(false);
            StaticParameters.GameType = GameTypes.Photon;

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
            _view.SetActive(true);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            _view.SetActive(true);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            _view.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            _view.SetActive(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            SceneManager.LoadScene(0);
        }

        public void CreateRoom(string roomName)
        {
            _view.SetActive(false);

            PhotonNetwork.CreateRoom(roomName, new RoomOptions
            {
                MaxPlayers = 4,
                IsVisible = true,
                IsOpen = true
            });
        }

        public void JoinRoom(string roomName)
        {
            _view.SetActive(false);

            if (string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.JoinRandomRoom();
                return;
            }

            PhotonNetwork.JoinRoom(roomName);
        }

        public void Quit()
        {
            _view.SetActive(false);
            PhotonNetwork.Disconnect();
        }
    }
}