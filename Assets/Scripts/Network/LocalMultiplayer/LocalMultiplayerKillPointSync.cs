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
            InteractRpc(speed, clientId);
        }

        [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
        private void InteractRpc(float speed, ulong clientId)
        {
            if (NetworkManager.LocalClientId == clientId)
                return;
            
            _killPoint.StartAnimation(speed);
        }
    }
}