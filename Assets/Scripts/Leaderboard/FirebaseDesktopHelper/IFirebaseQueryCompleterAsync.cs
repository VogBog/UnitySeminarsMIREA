using System.Threading.Tasks;

namespace Leaderboard.FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleter
    {
        Task AsyncEmpty();
        Task<string> AsyncString();
        Task<T> AsyncObject<T>();
    }
}