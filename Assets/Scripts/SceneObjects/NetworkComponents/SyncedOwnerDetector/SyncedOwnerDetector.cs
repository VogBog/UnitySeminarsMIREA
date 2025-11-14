using Data;
using MainMenu;
using Unity.Netcode;
using UnityEngine;

namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public class SyncedOwnerDetector : IOwnerDetector, ILocalMultiplayerOwnerDetector
    {
        private readonly IOwnerDetector _detector;

        public bool IsMy => _detector.IsMy;
        public bool IsServer => _detector.IsServer;
        
        public SyncedOwnerDetector(GameObject gameObject)
        {
            if (StaticParameters.NetworkType is NetworkTypes.LocalMultiplayer &&
                gameObject.TryGetComponent<NetworkObject>(out var networkObject))
            {
                _detector = new LocalMultiplayerOwnerDetector(networkObject);
            }
            else
            {
                _detector = new DefaultOwnerDetector();
            }
        }

        public void SetOwner(ulong ownerId)
        {
            if(_detector is ILocalMultiplayerOwnerDetector lmDetector)
                lmDetector.SetOwner(ownerId);
        }
    }
}