using Global;
using Photon.Pun;
using UnityEngine;

namespace Game.PhotonMultiplayer
{
    public class PhotonStartTimer : MonoBehaviour
    {
        private GameUI _gameUi;
        private PhotonView _photonView;
        
        private void Awake()
        {
            if (StaticParameters.NetworkType is not StaticParameters.NetworkTypes.Photon)
            {
                Destroy(this);
                return;
            }
            
            _photonView = GetComponent<PhotonView>();

            var players = FindObjectsByType<CarMovement>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                if (player.TryGetComponent(out PhotonView photonView) &&
                    photonView.IsMine)
                {
                    _gameUi = player.GetComponentInChildren<GameUI>(true);
                    if (PhotonNetwork.IsMasterClient)
                    {
                        _gameUi.TimerChanged += OnTimerChanged;
                    }

                    break;
                }
            }
        }

        private void OnTimerChanged(int value)
        {
            _photonView.RPC(nameof(TimerChangedRpc), RpcTarget.All, value);
        }

        [PunRPC]
        private void TimerChangedRpc(int value)
        {
            if (PhotonNetwork.IsMasterClient)
                return;
            _gameUi?.StopTimerAndUpdate(value);
        }
    }
}