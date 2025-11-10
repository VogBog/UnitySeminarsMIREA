using System;
using System.Collections.Generic;
using MainMenu;
using Player.NetworkPolitics;
using UnityEngine;

namespace SceneObjects.NetworkComponents
{
    public class SyncedPlayer : MultiNetworkSyncComponent
    {
        protected override void GetRequiredComponents(IList<Type> components)
        {
            components.Add(typeof(LocalMultiplayerNetworkPolitics));
        }

        protected override void DeactivateUselessComponents(NetworkTypes type, IDictionary<Type, Component> components)
        {
            if(components.TryGetValue(typeof(LocalMultiplayerNetworkPolitics), out var politics) &&
               type is not NetworkTypes.LocalMultiplayer)
                Destroy(politics);
        }
    }
}