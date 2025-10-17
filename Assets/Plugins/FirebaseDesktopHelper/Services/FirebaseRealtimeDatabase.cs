using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services
{
    public class FirebaseRealtimeDatabase
    {
        public string Url => FirebaseRestRequests.RealtimeDatabaseUrl;
        public string ApiKey => FirebaseRestRequests.ApiKey;

        public FirebaseRestQuery Query()
        {
            string idToken = FirebaseRestRequests.Tokens.IdToken;
            if (string.IsNullOrEmpty(idToken))
                return new(Url, ApiKey);
            return new(Url, idToken);
        }
        
        public static void LogError(string methodName, UnityWebRequest request) =>
            FirebaseRestRequests.LogError(methodName, request);

        public Task<(string, UnityWebRequest.Result)> SendRequest(UnityWebRequest request, string methodName)
            => FirebaseRestRequests.SendRequest(request, methodName);

        public async Task<(string, UnityWebRequest.Result)> SendGetQuery(FirebaseRestQuery query)
        {
            string url = query.Result;
            
            using var request = UnityWebRequest.Get(url);
            
            return await SendRequest(request, "SendGetQuery");
        }

        public async Task<(string, UnityWebRequest.Result)> SendPostQuery(FirebaseRestQuery query)
        {
            string url = query.Result;
            
            using var request = UnityWebRequest.Post(url, query.Json, "application/json");
            
            return await SendRequest(request, "SendPostQuery");
        }

        public async Task<(string, UnityWebRequest.Result)> SendPutQuery(FirebaseRestQuery query)
        {
            string url = query.Result;
            
            using var request = UnityWebRequest.Post(url, query.Json, "application/json");
            request.method = "PUT";
            
            return await SendRequest(request, "SendPutQuery");
        }

        public async Task<(string, UnityWebRequest.Result)> SendDeleteQuery(FirebaseRestQuery query)
        {
            string url = query.Result;
            
            using var request = UnityWebRequest.Delete(url);
            
            return await SendRequest(request, "SendDeleteQuery");
        }
    }
}