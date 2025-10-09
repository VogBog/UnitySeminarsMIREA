using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Leaderboard.FirebaseDesktopHelper.CompletersAsync
{
    public readonly struct FirebaseQueryCompleterPostAsync : IFirebaseQueryCompleterAsync
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterPostAsync(FirebaseRestQuery query)
        {
            _query = query;
        }


        public Task Empty()
        {
            return FirebaseRestRequests.SendPostQuery(_query);
        }

        public Task<string> String()
        {
            return FirebaseRestRequests.SendPostQuery(_query);
        }

        public async Task<T> Object<T>()
        {
            string json = await FirebaseRestRequests.SendPostQuery(_query);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        
        public async Task<T> ObjectWrapped<T>(string wrappedName)
        {
            string json = await FirebaseRestRequests.SendPostQuery(_query);
            json = $"{{\"{wrappedName}\": {json}}}";
            var obj = JsonConvert.DeserializeObject<T>(json);
            
            return obj;
        }
    }
}