using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    public readonly struct FirebaseSignUpCallback
    {
        private readonly string _json;

        public FirebaseSignUpCallback(string json)
        {
            _json = json;
        }

        public async Task Empty(Action<UnityWebRequest.Result> onComplete)
        {
            var (_, status) = await FirebaseRestRequests.Authentication.SignUpJson(_json);
            onComplete?.Invoke(status);
        }

        public async Task Json(Action<string, UnityWebRequest.Result> onComplete)
        {
            var (responseJson, status) = await FirebaseRestRequests.Authentication.SignUpJson(_json);
            onComplete?.Invoke(responseJson, status);
        }

        public async Task Response(Action<SignUpResponse, UnityWebRequest.Result> onComplete)
        {
            var (response, status) = await FirebaseRestRequests.Authentication.SignUpResponse(_json);
            onComplete?.Invoke(response, status);
        }
    }
}