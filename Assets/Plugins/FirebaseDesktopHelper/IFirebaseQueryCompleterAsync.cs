using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterAsync
    {
        Task<UnityWebRequest.Result> Empty();
        Task<(string, UnityWebRequest.Result)> String();
        Task<(T, UnityWebRequest.Result)> Object<T>();
        Task<(T, UnityWebRequest.Result)> ObjectWrapped<T>(string wrappedName);
    }
}