using System;
using LocalMultiplayerPage;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PhotonMultiplayerPage
{
    public class PhotonMultiplayerLobby : MonoBehaviourPunCallbacks, ILobby
    {
        [SerializeField] private LocalMultiplayerLobbyView _view;

        private bool _started = false;

        public event Action<int> PlayersCountChanged; 
        public event Action Quitted;

        private void Awake()
        {
            _view.Initialize(this, "");

            _view.StartClicked += TryStart;
            _view.QuitClicked += Quit;
            
            _view.HidePage();
        }

        public string GetRoomName() => PhotonNetwork.CurrentRoom?.Name;

        public void ShowLobby()
        {
            _view.ShowPage();
            PlayersCountChanged?.Invoke(PhotonNetwork.CurrentRoom?.PlayerCount ?? 0);
        }

        public void TryStart()
        {
            if (_started || !PhotonNetwork.IsMasterClient)
                return;
            _started = true;
            _view.HidePage();

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }

        public void Quit()
        {
            _view.HidePage();
            Quitted?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            PlayersCountChanged?.Invoke(PhotonNetwork.CurrentRoom?.PlayerCount ?? 0);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            PlayersCountChanged?.Invoke(PhotonNetwork.CurrentRoom?.PlayerCount ?? 0);
        }
    }
}