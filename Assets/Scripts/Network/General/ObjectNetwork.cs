using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Network.General
{
    public class ObjectNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(NetworkObject));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var networkObject = GetComponent<NetworkObject>();
            Destroy(networkObject);
        }
    }
}