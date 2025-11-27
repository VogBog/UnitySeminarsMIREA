using Game;
using Photon.Pun;
using UnityEngine;

namespace Network.PhotonMultiplayer
{
    public class PhotonMultiplayerKillPointSync : MonoBehaviour
    {
        private PhotonView _photonView;
        private KillPoint _killPoint;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            
            _killPoint = GetComponent<KillPoint>();
            _killPoint.InteractedWithSpeed += OnInteractedWithSpeed;
        }
        
        private void OnInteractedWithSpeed(KillPoint killPoint, float speed)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            _photonView.RPC(nameof(InteractRpc), RpcTarget.All, speed, actorNumber);
        }

        [PunRPC]
        private void InteractRpc(float speed, int actorNumber)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
                return;
            
            _killPoint.StartAnimation(speed);
        }
    }
}