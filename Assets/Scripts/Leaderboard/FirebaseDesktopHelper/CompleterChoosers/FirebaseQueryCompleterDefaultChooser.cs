using Leaderboard.FirebaseDesktopHelper.CompletersCallback;
using Leaderboard.FirebaseDesktopHelper.CompletersCoroutine;

namespace Leaderboard.FirebaseDesktopHelper.CompleterChoosers
{
    public readonly struct FirebaseQueryCompleterDefaultChooser : IFirebaseQueryCompleterChooser
    {
        private readonly IFirebaseQueryCompleterAsync _completerAsync;

        public FirebaseQueryCompleterDefaultChooser(IFirebaseQueryCompleterAsync completerAsync)
        {
            _completerAsync = completerAsync;
        }

        public IFirebaseQueryCompleterAsync Async() => _completerAsync;

        public IFirebaseQueryCompleterCallback Callback() => new FirebaseQueryCompleterDefaultCallback(Async());

        public IFirebaseQueryCompleterIEnumerator Coroutine() => new FirebaseQueryCompleterDefaultCoroutine(Callback());
    }
}