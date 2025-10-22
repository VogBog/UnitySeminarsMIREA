using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.CompletersCoroutine
{
    public readonly struct FirebaseQueryCompleterDefaultCoroutine : IFirebaseQueryCompleterIEnumerator
    {
        private readonly IFirebaseQueryCompleterCallback _completer;

        public FirebaseQueryCompleterDefaultCoroutine(IFirebaseQueryCompleterCallback callback)
        {
            _completer = callback;
        }

        public IEnumerator Empty()
        {
            var boolRef = new Reference<bool>();
            yield return _completer.Empty(_ => boolRef.Value = true);
            yield return new WaitUntil(() => boolRef.Value);
        }

        public IEnumerator RequestResult(Action<UnityWebRequest.Result> callback)
        {
            var statusRef = new ReferenceWithFlag<UnityWebRequest.Result>();
            yield return _completer.Empty(status => statusRef.Set(status));
            yield return new WaitUntil(() => statusRef.IsReady);
            
            callback?.Invoke(statusRef.Value);
        }

        public IEnumerator String(Action<string, UnityWebRequest.Result> callback)
        {
            var resRef = new ReferenceWithFlag<(string, UnityWebRequest.Result)>();
            yield return _completer.String((str, status) => resRef.Set((str, status)));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value.Item1, resRef.Value.Item2);
        }

        public IEnumerator Object<T>(Action<T, UnityWebRequest.Result> callback)
        {
            var resRef = new ReferenceWithFlag<(T, UnityWebRequest.Result)>();
            yield return _completer.Object<T>((t, status) => resRef.Set((t, status)));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value.Item1, resRef.Value.Item2);
        }

        public IEnumerator ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback)
        {
            var resRef = new ReferenceWithFlag<(T, UnityWebRequest.Result)>();
            yield return _completer.ObjectWrapped<T>(wrapperName, (obj, status) => resRef.Set((obj, status)));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value.Item1, resRef.Value.Item2);
        }
    }
}