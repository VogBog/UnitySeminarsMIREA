using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.PlayerKillers
{
    public class PhotonPlayerKiller : MonoBehaviour, IPlayerKiller
    {
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;

        public bool IsServer() => PhotonNetwork.IsMasterClient;
    }
}