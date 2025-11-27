using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Netcode.Components;

namespace Network.General
{
    public class TransformNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkTransform));
            components.Add(typeof(PhotonTransformView));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var network = GetComponent<NetworkTransform>();
            Destroy(network);
        }

        protected override void IsNotPhotonMultiplayer()
        {
            var network = GetComponent<PhotonTransformView>();
            Destroy(network);
        }
    }
}