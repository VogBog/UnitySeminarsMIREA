using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirebaseDesktopHelper.CompletersAsync
{
    public readonly struct FirebaseQueryCompleterDeleteAsync : IFirebaseQueryCompleterAsync
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterDeleteAsync(FirebaseRestQuery query)
        {
            _query = query;
        }

        public Task Empty()
        {
            return FirebaseRestRequests.SendDeleteQuery(_query);
        }

        public Task<string> String()
        {
            return FirebaseRestRequests.SendDeleteQuery(_query);
        }

        public async Task<T> Object<T>()
        {
            string json = await FirebaseRestRequests.SendDeleteQuery(_query);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public async Task<T> ObjectWrapped<T>(string wrappedName)
        {
            string json = await FirebaseRestRequests.SendDeleteQuery(_query);
            json = $"{{\"{wrappedName}\": {json}}}";
            var obj = JsonConvert.DeserializeObject<T>(json);
            
            return obj;
        }
    }
}