using Data;
using MainMenu;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerNetworkObject : MonoBehaviour
    {
        private void Start()
        {
            if (StaticParameters.NetworkType is not NetworkTypes.LocalMultiplayer)
            {
                var networkObject = GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    Destroy(networkObject);
                }
            }
        }
    }
}