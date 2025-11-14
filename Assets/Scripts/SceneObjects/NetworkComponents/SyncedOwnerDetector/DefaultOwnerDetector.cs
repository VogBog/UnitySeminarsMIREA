namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public class DefaultOwnerDetector : IOwnerDetector
    {
        public bool IsMy => true;
        public bool IsServer => true;
    }
}