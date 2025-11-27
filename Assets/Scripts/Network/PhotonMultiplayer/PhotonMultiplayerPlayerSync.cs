using System.Collections;
using Game;
using Photon.Pun;
using UnityEngine;

namespace Network.PhotonMultiplayer
{
    public class PhotonMultiplayerPlayerSync : MonoBehaviour
    {
        private Player _player;
        private PhotonView _photonView;
        
        private IEnumerator Start()
        {
            _player = GetComponent<Player>();
            _photonView = GetComponent<PhotonView>();
            
            if (_photonView.IsMine)
            {
                while(_player.Movement == null)
                    yield return null;
                _player.Movement.StoppedChanged += OnOwnerStoppedChanged;
            }
        }

        private void OnOwnerStoppedChanged(bool stopped)
        {
            _photonView.RPC(nameof(StoppedChangedRpc), RpcTarget.All, stopped);
        }
        
        [PunRPC]
        private void StoppedChangedRpc(bool value)
        {
            if (_photonView.IsMine)
                return;
            
            _player.Movement.Marker.gameObject.SetActive(value);
        }
    }
}