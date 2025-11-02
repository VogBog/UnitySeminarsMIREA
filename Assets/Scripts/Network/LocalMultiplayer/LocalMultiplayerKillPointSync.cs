using Game;
using Unity.Netcode;
using UnityEngine;

namespace Network.LocalMultiplayer
{
    [RequireComponent(typeof(KillPoint))]
    public class LocalMultiplayerKillPointSync : NetworkBehaviour
    {
        private KillPoint _killPoint;
        
        private void Start()
        {
            _killPoint = GetComponent<KillPoint>();
            _killPoint.InteractedWithSpeed += OnInteractedWithSpeed;
        }

        private void OnInteractedWithSpeed(KillPoint killPoint, float speed)
        {
            ulong clientId = NetworkManager.LocalClientId;
            if (IsServer)
            {
                InteractClientRpc(speed, clientId);
            }
            else
            {
                InteractServerRpc(speed, clientId);
            }
        }

        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        private void InteractServerRpc(float speed, ulong clientId)
        {
            InteractClientRpc(speed, clientId);
        }

        [ClientRpc]
        private void InteractClientRpc(float speed, ulong clientId)
        {
            if (NetworkManager.LocalClientId == clientId)
                return;
            
            _killPoint.StartAnimation(speed);
        }
    }
}