using System;
using System.Collections;
using UnityEngine;

namespace Leaderboard.FirebaseDesktopHelper.CompletersCoroutine
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
            yield return _completer.Empty(() => boolRef.Value = true);
            yield return new WaitUntil(() => boolRef.Value);
        }

        public IEnumerator String(Action<string> callback)
        {
            var resRef = new ReferenceWithFlag<string>();
            yield return _completer.String(str => resRef.Set(str));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value);
        }

        public IEnumerator Object<T>(Action<T> callback)
        {
            var resRef = new ReferenceWithFlag<T>();
            yield return _completer.Object<T>(obj => resRef.Set(obj));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value);
        }

        public IEnumerator ObjectWrapped<T>(string wrapperName, Action<T> callback)
        {
            var resRef = new ReferenceWithFlag<T>();
            yield return _completer.ObjectWrapped<T>(wrapperName, obj => resRef.Set(obj));
            yield return new WaitUntil(() => resRef.IsReady);
            
            callback?.Invoke(resRef.Value);
        }
    }
}