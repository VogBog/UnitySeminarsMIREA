using Unity.Netcode;

namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public class LocalMultiplayerOwnerDetector : IOwnerDetector, ILocalMultiplayerOwnerDetector
    {
        private readonly NetworkObject _networkObject;
        private ulong _ownerId;

        public bool IsMy => NetworkManager.Singleton.LocalClientId == _ownerId;
        public bool IsServer => NetworkManager.Singleton.IsServer;

        public LocalMultiplayerOwnerDetector(NetworkObject obj)
        {
            _networkObject = obj;
            _ownerId = obj.OwnerClientId;
        }

        public void SetOwner(ulong ownerId)
        {
            _ownerId = ownerId;
        }
    }
}