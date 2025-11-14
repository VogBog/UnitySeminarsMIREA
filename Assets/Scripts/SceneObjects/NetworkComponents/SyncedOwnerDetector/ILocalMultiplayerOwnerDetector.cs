namespace SceneObjects.NetworkComponents.SyncedOwnerDetector
{
    public interface ILocalMultiplayerOwnerDetector
    {
        void SetOwner(ulong ownerId);
    }
}