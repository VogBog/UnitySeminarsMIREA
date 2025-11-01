using Network.LocalMultiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(LocalMultiplayerPlayerSync))]
    public class PlayerNetwork : AbstractNetwork
    {
        protected override bool DestroyAfterAwake => false;
        
        public NetworkObject NetworkObject { get; private set; }

        protected override void BeforeAwake()
        {
            NetworkObject = GetComponent<NetworkObject>();
        }

        protected override void IsNotLocalMultiplayer()
        {
            var playerSync = GetComponent<LocalMultiplayerPlayerSync>();
            Destroy(playerSync);
        }
    }
}