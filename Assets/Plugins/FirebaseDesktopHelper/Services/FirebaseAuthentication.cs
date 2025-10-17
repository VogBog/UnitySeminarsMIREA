using System.Threading.Tasks;
using FirebaseDesktopHelper.Services.Auth.SignIn;
using FirebaseDesktopHelper.Services.Auth.SignUp;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services
{
    public class FirebaseAuthentication
    {
        public string ApiKey => FirebaseRestRequests.ApiKey;
        public string SignUpUrl => $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";
        public bool AutoUpdateTokens = true;

        public string SignInWithPasswordUrl =>
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";

        public FirebaseSignUpMethodChooser SignUp() => new();
        public FirebaseSignInMethodChooser SignIn() => new();

        private UnityWebRequest CreateWebRequest(string url, string json)
        {
            var request = UnityWebRequest.Post(url, json, "application/json");
            return request;
        }

        public async Task<(string, UnityWebRequest.Result)> SignUpJson(string json)
        {
            using var request = CreateWebRequest(SignUpUrl, json);

            var pair = await FirebaseRestRequests.SendRequest(request, "SignUpJson");
            
            if (AutoUpdateTokens)
            {
                var raw = JsonConvert.DeserializeObject<SignUpResponseRaw>(pair.Item1);
                FirebaseRestRequests.Tokens.SetIdAndRefreshToken(
                    raw.idToken, raw.refreshToken, Mathf.RoundToInt(raw.expiresIn));
            }
            
            return pair;
        }

        public async Task<(SignUpResponse, UnityWebRequest.Result)> SignUpResponse(string json)
        {
            using var request = CreateWebRequest(SignUpUrl, json);
            
            var (response, status) = await
                FirebaseRestRequests.SendRequest(request, "SignUpResponse");

            try
            {
                var raw = JsonConvert.DeserializeObject<SignUpResponseRaw>(response);
                var obj = raw.Convert();

                if (AutoUpdateTokens)
                {
                    FirebaseRestRequests.Tokens.SetIdAndRefreshToken(
                        obj.IdToken, obj.RefreshToken, Mathf.RoundToInt(obj.ExpiresIn));
                }
                
                return (obj, status);
            }
            catch (JsonReaderException e)
            {
                Debug.LogException(e);
                return (default, UnityWebRequest.Result.DataProcessingError);
            }
        }

        public async Task<(string, UnityWebRequest.Result)> SignInJson(string json)
        {
            using var request = CreateWebRequest(SignInWithPasswordUrl, json);
            
            var pair = await FirebaseRestRequests.SendRequest(request, "SignInJson");

            if (AutoUpdateTokens)
            {
                var raw = JsonConvert.DeserializeObject<SignInResponseRaw>(pair.Item1);
                FirebaseRestRequests.Tokens.SetIdAndRefreshToken(
                    raw.idToken, raw.refreshToken, Mathf.RoundToInt(raw.expiresIn));
            }
            
            return pair;
        }

        public async Task<(SignInResponse, UnityWebRequest.Result)> SignInResponse(string json)
        {
            using var request = CreateWebRequest(SignInWithPasswordUrl, json);
            
            var (response, status) = await
                FirebaseRestRequests.SendRequest(request, "SignInResponse");

            try
            {
                var raw = JsonConvert.DeserializeObject<SignInResponseRaw>(response);
                var obj = raw.Convert();

                if (AutoUpdateTokens)
                {
                    FirebaseRestRequests.Tokens.SetIdAndRefreshToken(
                        obj.IdToken, obj.RefreshToken, Mathf.RoundToInt(obj.ExpiresIn));
                }
                
                return (obj, status);
            }
            catch (JsonReaderException e)
            {
                Debug.LogException(e);
                return (default, UnityWebRequest.Result.DataProcessingError);
            }
        }
    }
}