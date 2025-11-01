using Unity.Netcode.Components;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(NetworkTransform))]
    public class TransformNetwork : AbstractNetwork
    {
        protected override void IsNotLocalMultiplayer()
        {
            var network = GetComponent<NetworkTransform>();
            Destroy(network);
        }
    }
}