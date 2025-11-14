using System;
using System.Collections.Generic;
using Global;
using Unity.Netcode.Components;
using UnityEngine;

namespace MultiNetwork.SyncedRigidBody
{
    public class SyncedRigidBody : MultiNetworkComponent.MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(Rigidbody));
            components.Add(typeof(NetworkRigidbody));
        }

        protected override void RemoveRedundantComponents(StaticParameters.NetworkTypes type, Dictionary<Type, Component> components)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                components.TryGetValue(typeof(NetworkRigidbody), out var network))
            {
                Destroy(network);
            }
        }
    }
}