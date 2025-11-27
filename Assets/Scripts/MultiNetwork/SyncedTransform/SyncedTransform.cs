using System;
using System.Collections.Generic;
using Global;
using Photon.Pun;
using Unity.Netcode.Components;
using UnityEngine;

namespace MultiNetwork.SyncedTransform
{
    public class SyncedTransform : MultiNetworkComponent.MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkTransform));
            components.Add(typeof(PhotonTransformView));
        }

        protected override void RemoveRedundantComponents(StaticParameters.NetworkTypes type, Dictionary<Type, Component> components)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                components.TryGetValue(typeof(NetworkTransform), out var networkTransform))
            {
                Destroy(networkTransform);
            }

            if (type is not StaticParameters.NetworkTypes.Photon &&
                components.TryGetValue(typeof(PhotonTransformView), out var photonView))
            {
                Destroy(photonView);
            }
        }
    }
}