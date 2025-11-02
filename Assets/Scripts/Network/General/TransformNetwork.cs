using System;
using System.Collections.Generic;
using Unity.Netcode.Components;

namespace Network.General
{
    public class TransformNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkTransform));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var network = GetComponent<NetworkTransform>();
            Destroy(network);
        }
    }
}