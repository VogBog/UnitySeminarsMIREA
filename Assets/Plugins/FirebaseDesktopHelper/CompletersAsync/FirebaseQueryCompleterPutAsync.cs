using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirebaseDesktopHelper.CompletersAsync
{
    public readonly struct FirebaseQueryCompleterPutAsync : IFirebaseQueryCompleterAsync
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterPutAsync(FirebaseRestQuery query)
        {
            _query = query;
        }

        public Task Empty()
        {
            return FirebaseRestRequests.SendPutQuery(_query);
        }

        public Task<string> String()
        {
            return FirebaseRestRequests.SendPutQuery(_query);
        }

        public async Task<T> Object<T>()
        {
            string json = await FirebaseRestRequests.SendPutQuery(_query);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        
        public async Task<T> ObjectWrapped<T>(string wrappedName)
        {
            string json = await FirebaseRestRequests.SendPutQuery(_query);
            json = $"{{\"{wrappedName}\": {json}}}";
            var obj = JsonConvert.DeserializeObject<T>(json);
            
            return obj;
        }
    }
}