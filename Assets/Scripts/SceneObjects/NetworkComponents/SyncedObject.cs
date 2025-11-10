using System;
using System.Collections.Generic;
using MainMenu;
using Unity.Netcode;
using UnityEngine;

namespace SceneObjects.NetworkComponents
{
    public class SyncedObject : MultiNetworkSyncComponent
    {
        protected override void GetRequiredComponents(IList<Type> components)
        {
            components.Add(typeof(NetworkObject));
        }

        protected override void DeactivateUselessComponents(NetworkTypes type, IDictionary<Type, Component> components)
        {
            if(components.TryGetValue(typeof(NetworkObject), out var networkObject) &&
               type is not NetworkTypes.LocalMultiplayer)
                Destroy(networkObject);
        }
    }
}