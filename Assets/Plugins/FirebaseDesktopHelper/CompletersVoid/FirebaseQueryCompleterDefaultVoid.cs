using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.CompletersVoid
{
    public readonly struct FirebaseQueryCompleterDefaultVoid : IFirebaseQueryCompleterVoid
    {
        private readonly IFirebaseQueryCompleterAsync _completer;

        public FirebaseQueryCompleterDefaultVoid(IFirebaseQueryCompleterAsync completer)
        {
            _completer = completer;
        }

        public async void Empty()
        {
            try
            {
                await _completer.Empty();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void Request(Action<UnityWebRequest.Result> callback)
        {
            try
            {
                var status = await _completer.Empty();
                callback?.Invoke(status);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void String(Action<string, UnityWebRequest.Result> callback)
        {
            try
            {
                var (res, status) = await _completer.String();
                callback?.Invoke(res, status);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void Object<T>(Action<T, UnityWebRequest.Result> callback)
        {
            try
            {
                var (res, status) = await _completer.Object<T>();
                callback?.Invoke(res, status);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback)
        {
            try
            {
                var (res, status) = await _completer.ObjectWrapped<T>(wrapperName);
                callback?.Invoke(res, status);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}