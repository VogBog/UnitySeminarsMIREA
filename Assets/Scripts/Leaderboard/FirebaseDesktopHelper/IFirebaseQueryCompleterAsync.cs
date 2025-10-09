using System.Threading.Tasks;

namespace Leaderboard.FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterAsync
    {
        Task Empty();
        Task<string> String();
        Task<T> Object<T>();
        Task<T> ObjectWrapped<T>(string wrappedName);
    }
}