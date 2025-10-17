using System.Threading.Tasks;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    public readonly struct FirebaseSignUpAsync
    {
        private readonly string _json;

        public FirebaseSignUpAsync(string json)
        {
            _json = json;
        }

        public async Task<UnityWebRequest.Result> Empty()
        {
            var (_, status) = await FirebaseRestRequests.Authentication.SignUpJson(_json);
            return status;
        }

        public Task<(string, UnityWebRequest.Result)> Json()
        {
            return FirebaseRestRequests.Authentication.SignUpJson(_json);
        }

        public Task<(SignUpResponse, UnityWebRequest.Result)> Response()
        {
            return FirebaseRestRequests.Authentication.SignUpResponse(_json);
        }
    }
}