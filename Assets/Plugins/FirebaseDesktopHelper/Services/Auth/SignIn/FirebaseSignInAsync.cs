using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    public readonly struct FirebaseSignInAsync
    {
        private readonly string _json;

        public FirebaseSignInAsync(string json)
        {
            _json = json;
        }
        
        public async Task<UnityWebRequest.Result> Empty()
        {
            var (_, status) = await FirebaseRestRequests.Authentication.SignInJson(_json);
            return status;
        }

        public Task<(string, UnityWebRequest.Result)> Json()
        {
            return FirebaseRestRequests.Authentication.SignInJson(_json);
        }

        public Task<(SignInResponse, UnityWebRequest.Result)> Response()
        {
            return FirebaseRestRequests.Authentication.SignInResponse(_json);
        }
    }
}