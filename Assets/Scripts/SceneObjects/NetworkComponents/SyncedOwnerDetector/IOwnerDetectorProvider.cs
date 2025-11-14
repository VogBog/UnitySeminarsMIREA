namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public interface IOwnerDetectorProvider
    {
        IOwnerDetector OwnerDetector { get; }
    }
}