using System;
using System.Collections.Generic;
using Base;
using Unity.Netcode;

namespace MultiNetwork
{
    public class SyncedObject : MultiNetworkComponent
    {
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkObject));
        }

        protected override void DestroyRedundantComponents(StaticParameters.NetworkTypes type)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                TryGetComponent(out NetworkObject networkObject))
            {
                Destroy(networkObject);
            }
        }
    }
}