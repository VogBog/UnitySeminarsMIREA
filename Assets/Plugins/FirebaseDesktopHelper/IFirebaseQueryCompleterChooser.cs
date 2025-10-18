namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterChooser
    {
        IFirebaseQueryCompleterAsync Async();
        IFirebaseQueryCompleterCallback Callback();
        IFirebaseQueryCompleterIEnumerator Coroutine();
        IFirebaseQueryCompleterVoid Void();
    }
}