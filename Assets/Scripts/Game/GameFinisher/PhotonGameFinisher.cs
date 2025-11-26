using System;
using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.GameFinisher
{
    public class PhotonGameFinisher : MonoBehaviour, IGameFinisher
    {
        private PhotonView _photonView;
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;
        
        public event Action Died;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public bool IsServer() => PhotonNetwork.IsMasterClient;

        public void InvokeDiedToOwner(Player.Player player)
        {
            var photonView = player.GetComponentInChildren<PhotonView>();
            if (photonView == null)
            {
                Debug.LogError("Player has no photonView");
                return;
            }

            int actorNumber = photonView.Owner.ActorNumber;
            _photonView.RPC(nameof(InvokeDiedToOwnerRpc), RpcTarget.All, actorNumber);
        }

        private void InvokeDiedToOwnerRpc(int actorNumber)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
                return;
            
            Died?.Invoke();
        }

        public void QuitFromGame()
        {
            PhotonNetwork.Disconnect();
        }
    }
}