using System;
using System.Collections.Generic;
using Base;
using Photon.Pun;
using Unity.Netcode.Components;

namespace MultiNetwork
{
    public class SyncedTransform : MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkTransform));
            components.Add(typeof(PhotonTransformView));
        }

        protected override void DestroyRedundantComponents(StaticParameters.NetworkTypes type)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                TryGetComponent(out NetworkTransform networkTransform))
            {
                Destroy(networkTransform);
            }

            if (type is not StaticParameters.NetworkTypes.Photon &&
                TryGetComponent(out PhotonTransformView photonTransformView))
            {
                Destroy(photonTransformView);
            }
        }
    }
}