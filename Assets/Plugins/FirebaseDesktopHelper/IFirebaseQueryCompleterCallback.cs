using System;
using System.Threading.Tasks;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterCallback
    {
        Task Empty(Action callback);
        Task String(Action<string> callback);
        Task Object<T>(Action<T> callback);
        Task ObjectWrapped<T>(string wrapperName, Action<T> callback);
    }
}