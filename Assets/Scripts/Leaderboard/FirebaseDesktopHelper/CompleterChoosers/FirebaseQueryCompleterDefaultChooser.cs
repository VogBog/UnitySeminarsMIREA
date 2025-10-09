using Leaderboard.FirebaseDesktopHelper.CompletersAsync;
using Leaderboard.FirebaseDesktopHelper.CompletersCallback;

namespace Leaderboard.FirebaseDesktopHelper.CompleterChoosers
{
    public readonly struct FirebaseQueryCompleterGetChooser : IFirebaseQueryCompleterChooser
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterGetChooser(FirebaseRestQuery query)
        {
            _query = query;
        }

        public IFirebaseQueryCompleterAsync Async() => new FirebaseQueryCompleterGetAsync(_query);

        public IFirebaseQueryCompleterCallback Callback() => new FirebaseQueryCompleterDefaultCallback(Async());

        public IFirebaseQueryCompleterIEnumerator Coroutine()
        {
            throw new System.NotImplementedException();
        }
    }
}