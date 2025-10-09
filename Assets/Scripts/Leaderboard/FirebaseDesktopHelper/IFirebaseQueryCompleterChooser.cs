namespace Leaderboard.FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterChooser
    {
        IFirebaseQueryCompleterAsync Async();
        IFirebaseQueryCompleterCallback Callback();
        IFirebaseQueryCompleterIEnumerator Coroutine();
    }
}