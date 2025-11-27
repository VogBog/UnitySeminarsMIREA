using System;
using System.Collections.Generic;
using Global;
using Unity.Netcode.Components;
using UnityEngine;

namespace MultiNetwork.SyncedTransform
{
    public class SyncedTransform : MultiNetworkComponent.MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkTransform));
        }

        protected override void RemoveRedundantComponents(StaticParameters.NetworkTypes type, Dictionary<Type, Component> components)
        {
            if(type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
               components.TryGetValue(typeof(NetworkTransform), out var networkTransform))
                Destroy(networkTransform);
        }
    }
}