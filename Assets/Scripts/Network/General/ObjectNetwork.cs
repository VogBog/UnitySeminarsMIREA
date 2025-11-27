using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Netcode;

namespace Network.General
{
    public class ObjectNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkObject));
            components.Add(typeof(PhotonView));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var networkObject = GetComponent<NetworkObject>();
            Destroy(networkObject);
        }

        protected override void IsNotPhotonMultiplayer()
        {
            var photonView = GetComponent<PhotonView>();
            Destroy(photonView);
        }
    }
}