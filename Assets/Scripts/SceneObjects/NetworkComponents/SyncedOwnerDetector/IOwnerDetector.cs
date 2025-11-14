namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public interface IOwnerDetector
    {
        bool IsMy { get; }
        bool IsServer { get; }
    }
}