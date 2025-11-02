using System.Collections;
using Game;
using Unity.Netcode;

namespace Network.LocalMultiplayer
{
    public class LocalMultiplayerPlayerSync : NetworkBehaviour
    {
        private Player _player;
        
        private IEnumerator Start()
        {
            _player = GetComponent<Player>();
            if (IsOwner)
            {
                while(_player.Movement == null)
                    yield return null;
                _player.Movement.StoppedChanged += OnOwnerStoppedChanged;
            }
        }

        private void OnOwnerStoppedChanged(bool stopped)
        {
            StoppedChangedRpc(stopped);
        }

        [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
        private void StoppedChangedRpc(bool value)
        {
            if (IsOwner)
                return;
            
            _player.Movement.Marker.gameObject.SetActive(value);
        }
    }
}