using System;
using Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameFinisher
{
    public class PhotonGameFinisher : MonoBehaviourPunCallbacks, IGameFinisher
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

        [PunRPC]
        private void InvokeDiedToOwnerRpc(int actorNumber)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
                return;
            
            Died?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            SceneManager.LoadScene(0);
        }

        public void QuitFromGame()
        {
            PhotonNetwork.Disconnect();
        }

        public void ServerQuitFromGame()
        {
            _photonView.RPC(nameof(QuitFromGameClientRpc), RpcTarget.All);
        }

        [PunRPC]
        private void QuitFromGameClientRpc()
        {
            QuitFromGame();
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }
}