using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.PlayerKillers
{
    public class PhotonPlayerKiller : MonoBehaviour, IPlayerKiller
    {
        private PhotonView _photonView;
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public bool IsServer() => PhotonNetwork.IsMasterClient;
        
        public void InvokePlayerDied(Player.Player player)
        {
            var photonView = player.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                player.Die();
            }
            else
            {
                int actorId = photonView.Owner.ActorNumber;
                _photonView.RPC(nameof(InvokePlayerDiedRpc), RpcTarget.All, actorId);
            }
        }

        [PunRPC]
        private void InvokePlayerDiedRpc(int actorId)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorId)
                return;

            var players = FindObjectsByType<Player.Player>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                var photonView = player.GetComponent<PhotonView>();
                if (photonView.IsMine)
                {
                    player.Die();
                    return;
                }
            }
        }
    }
}