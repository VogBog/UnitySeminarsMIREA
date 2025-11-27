using Photon.Pun;
using UnityEngine;

namespace Game.PhotonMultiplayer
{
    public class PhotonGameFinisher : IGameFinisher
    {
        public CarMapRunner[] GetRunnersForRecord()
        {
            var runners = UnityEngine.Object.FindObjectsByType<CarMapRunner>(FindObjectsSortMode.None);
            foreach (var car in runners)
            {
                if (car.TryGetComponent(out PhotonView photonView) &&
                    photonView.IsMine)
                {
                    return new[] { car };
                }
            }

            return null;
        }

        public void OnBeforeLoadingScene()
        {
            PhotonNetwork.Disconnect();
        }
    }
}