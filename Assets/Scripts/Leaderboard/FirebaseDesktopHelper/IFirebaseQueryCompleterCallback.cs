using System;
using System.Threading.Tasks;

namespace Leaderboard.FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterCallback
    {
        Task Empty(Action callback);
        Task String(Action<string> callback);
        Task Object<T>(Action<T> callback);
    }
}