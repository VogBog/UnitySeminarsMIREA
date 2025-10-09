using System;
using System.Threading.Tasks;

namespace Leaderboard.FirebaseDesktopHelper.CompletersCallback
{
    public readonly struct FirebaseQueryCompleterDefaultCallback : IFirebaseQueryCompleterCallback
    {
        private readonly IFirebaseQueryCompleterAsync _completer;

        public FirebaseQueryCompleterDefaultCallback(IFirebaseQueryCompleterAsync completer)
        {
            _completer = completer;
        }

        public async Task Empty(Action callback)
        {
            await _completer.Empty();
            callback?.Invoke();
        }

        public async Task String(Action<string> callback)
        {
            string json = await _completer.String();
            callback?.Invoke(json);
        }

        public async Task Object<T>(Action<T> callback)
        {
            var obj = await _completer.Object<T>();
            callback?.Invoke(obj);
        }
    }
}