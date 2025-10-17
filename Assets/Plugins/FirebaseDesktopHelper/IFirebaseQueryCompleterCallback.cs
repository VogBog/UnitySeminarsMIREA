using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterCallback
    {
        Task Empty(Action<UnityWebRequest.Result> callback);
        Task String(Action<string, UnityWebRequest.Result> callback);
        Task Object<T>(Action<T, UnityWebRequest.Result> callback);
        Task ObjectWrapped<T>(string wrapperName, Action<T, UnityWebRequest.Result> callback);
    }
}