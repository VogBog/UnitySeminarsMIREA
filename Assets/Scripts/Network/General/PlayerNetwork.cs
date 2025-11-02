using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;
using Unity.Netcode;

namespace Network.General
{
    public class PlayerNetwork : AbstractNetwork
    {
        protected override bool DestroyAfterAwake => false;
        
        public NetworkObject NetworkObject { get; private set; }

        protected override void BeforeAwake()
        {
            NetworkObject = GetComponent<NetworkObject>();
        }

        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerPlayerSync));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var playerSync = GetComponent<LocalMultiplayerPlayerSync>();
            Destroy(playerSync);
        }
    }
}