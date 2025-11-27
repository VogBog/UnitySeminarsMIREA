using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;
using Network.PhotonMultiplayer;
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
            components.Add(typeof(PhotonMultiplayerPlayerSync));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var playerSync = GetComponent<LocalMultiplayerPlayerSync>();
            Destroy(playerSync);
        }

        protected override void IsNotPhotonMultiplayer()
        {
            var playerSync = GetComponent<PhotonMultiplayerPlayerSync>();
            Destroy(playerSync);
        }

        public bool IsOwner()
        {
            if (NetworkObject != null)
                return NetworkObject.IsOwner;

            return true;
        }
    }
}