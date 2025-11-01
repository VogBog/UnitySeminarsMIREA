using Unity.Netcode;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(NetworkObject))]
    public class ObjectNetwork : AbstractNetwork
    {
        protected override void IsNotLocalMultiplayer()
        {
            var networkObject = GetComponent<NetworkObject>();
            Destroy(networkObject);
        }
    }
}