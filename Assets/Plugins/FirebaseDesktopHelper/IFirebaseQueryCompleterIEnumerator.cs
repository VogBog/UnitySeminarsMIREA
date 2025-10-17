using System;
using System.Collections;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterIEnumerator
    {
        IEnumerator Empty();
        IEnumerator RequestResult(Action<UnityWebRequest.Result> callback);
        IEnumerator String(Action<string, UnityWebRequest.Result> callback);
        IEnumerator Object<T>(Action<T, UnityWebRequest.Result> callback);
        IEnumerator ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback);
    }
}