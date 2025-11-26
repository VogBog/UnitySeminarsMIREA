using System;
using System.Collections.Generic;
using Base;
using Photon.Pun;
using Unity.Netcode;

namespace MultiNetwork
{
    public class SyncedObject : MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkObject));
            components.Add(typeof(PhotonView));
        }

        protected override void DestroyRedundantComponents(StaticParameters.NetworkTypes type)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                TryGetComponent(out NetworkObject networkObject))
            {
                Destroy(networkObject);
            }

            if (type is not StaticParameters.NetworkTypes.Photon &&
                TryGetComponent(out PhotonView photonView))
            {
                Destroy(photonView);
            }
        }
    }
}