using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    public readonly struct FirebaseSignInCallback
    {
        private readonly string _json;

        public FirebaseSignInCallback(string json)
        {
            _json = json;
        }
        
        public async Task Empty(Action<UnityWebRequest.Result> onComplete)
        {
            var (_, status) = await FirebaseRestRequests.Authentication.SignInJson(_json);
            onComplete?.Invoke(status);
        }

        public async Task Json(Action<string, UnityWebRequest.Result> onComplete)
        {
            var (responseJson, status) = await FirebaseRestRequests.Authentication.SignInJson(_json);
            onComplete?.Invoke(responseJson, status);
        }

        public async Task Response(Action<SignInResponse, UnityWebRequest.Result> onComplete)
        {
            var (response, status) = await FirebaseRestRequests.Authentication.SignInResponse(_json);
            onComplete?.Invoke(response, status);
        }
    }
}