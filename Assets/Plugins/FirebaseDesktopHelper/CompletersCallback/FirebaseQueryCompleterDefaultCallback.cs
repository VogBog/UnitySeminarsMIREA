using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.CompletersCallback
{
    public readonly struct FirebaseQueryCompleterDefaultCallback : IFirebaseQueryCompleterCallback
    {
        private readonly IFirebaseQueryCompleterAsync _completer;

        public FirebaseQueryCompleterDefaultCallback(IFirebaseQueryCompleterAsync completer)
        {
            _completer = completer;
        }

        public async Task Empty(Action<UnityWebRequest.Result> callback)
        {
            var status = await _completer.Empty();
            callback?.Invoke(status);
        }

        public async Task String(Action<string, UnityWebRequest.Result> callback)
        {
            var (json, status) = await _completer.String();
            callback?.Invoke(json, status);
        }

        public async Task Object<T>(Action<T, UnityWebRequest.Result> callback)
        {
            var (obj, status) = await _completer.Object<T>();
            callback?.Invoke(obj, status);
        }

        public async Task ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback)
        {
            var (obj, status) = await _completer.ObjectWrapped<T>(wrapperName);
            callback?.Invoke(obj, status);
        }
    }
}