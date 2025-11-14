using System;
using System.Collections.Generic;
using Global;
using Unity.Netcode;
using UnityEngine;

namespace MultiNetwork.SyncedObject
{
    public class SyncedObject : MultiNetworkComponent.MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkObject));
        }

        protected override void RemoveRedundantComponents(StaticParameters.NetworkTypes type, Dictionary<Type, Component> components)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                components.TryGetValue(typeof(NetworkObject), out var networkObject))
            {
                Destroy(networkObject);
            }
        }
    }
}