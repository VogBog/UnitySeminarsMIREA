using System;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterVoid
    {
        void Empty();
        void Request(Action<UnityWebRequest.Result> callback);
        void String(Action<string, UnityWebRequest.Result> callback);
        void Object<T>(Action<T, UnityWebRequest.Result> callback);
        void ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback);
    }
}