using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirebaseDesktopHelper.CompletersAsync
{
    public readonly struct FirebaseQueryCompleterGetAsync : IFirebaseQueryCompleterAsync
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterGetAsync(FirebaseRestQuery query)
        {
            _query = query;
        }
        
        public Task Empty()
        {
            return FirebaseRestRequests.SendGetQuery(_query);
        }

        public Task<string> String()
        {
            return FirebaseRestRequests.SendGetQuery(_query);
        }

        public async Task<T> Object<T>()
        {
            string json = await FirebaseRestRequests.SendGetQuery(_query);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public async Task<T> ObjectWrapped<T>(string wrappedName)
        {
            string json = await FirebaseRestRequests.SendGetQuery(_query);
            json = $"{{\"{wrappedName}\": {json}}}";
            var obj = JsonConvert.DeserializeObject<T>(json);
            
            return obj;
        }
    }
}