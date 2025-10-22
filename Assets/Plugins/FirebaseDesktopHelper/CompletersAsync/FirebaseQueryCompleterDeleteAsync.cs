using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.CompletersAsync
{
    public readonly struct FirebaseQueryCompleterDeleteAsync : IFirebaseQueryCompleterAsync
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseQueryCompleterDeleteAsync(FirebaseRestQuery query)
        {
            _query = query;
        }

        public async Task<UnityWebRequest.Result> Empty()
        {
            var (_, status) = await FirebaseRestRequests.RealtimeDatabase.SendDeleteQuery(_query);
            return status;
        }

        public Task<(string, UnityWebRequest.Result)> String()
        {
            return FirebaseRestRequests.RealtimeDatabase.SendDeleteQuery(_query);
        }

        public async Task<(T, UnityWebRequest.Result)> Object<T>()
        {
            var (json, status) = await FirebaseRestRequests.RealtimeDatabase.SendDeleteQuery(_query);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return (obj, status);
        }

        public async Task<(T, UnityWebRequest.Result)> ObjectWrapped<T>(string wrappedName)
        {
            var (json, status) = await FirebaseRestRequests.RealtimeDatabase.SendDeleteQuery(_query);
            json = $"{{\"{wrappedName}\": {json}}}";
            var obj = JsonConvert.DeserializeObject<T>(json);
            
            return (obj, status);
        }
    }
}