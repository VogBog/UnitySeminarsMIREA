using System.Collections;
using Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonLobby
{
    public class PhotonLobbyFirstPage : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonLobbyFirstPageView _view;
        [SerializeField] private PhotonLobbySecondPage _nextPage;

        private bool _loading;
        
        private void Start()
        {
            StaticParameters.NetworkType = StaticParameters.NetworkTypes.Photon;
            _loading = true;
            _view.Initialize();

            _view.CreateClicked += CreateRoom;
            _view.JoinClicked += JoinRoom;
            _view.QuitClicked += Quit;
            
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "ru";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            
            _view.Show();
            _loading = false;
        }

        private void CreateRoom()
        {
            if (_loading)
                return;
            _loading = true;
            
            _view.Hide();
            var roomName = $"Room{Random.Range(100_000, 1_000_000)}:{Random.Range(100_000, 1_000_000)}";
            PhotonNetwork.CreateRoom(roomName, new RoomOptions()
            {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 10
            });
        }

        private void JoinRoom()
        {
            if (_loading)
                return;
            _loading = true;
            
            _view.Hide();
            PhotonNetwork.JoinRandomRoom();
        }

        private void Quit()
        {
            if (_loading)
                return;
            _loading = true;
            
            StartCoroutine(QuitRoutine());
        }

        private IEnumerator QuitRoutine()
        {
            _view.Hide();

            yield return null;
            
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            
            _view.Show();
            _loading = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            
            _view.Show();
            _loading = false;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            _nextPage.Activate();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            
            _nextPage.Activate();
        }
    }
}