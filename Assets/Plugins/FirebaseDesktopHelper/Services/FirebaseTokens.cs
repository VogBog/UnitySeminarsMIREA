using System;
using System.Threading;
using System.Threading.Tasks;
using FirebaseDesktopHelper.Services.Tokens;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Task = System.Threading.Tasks.Task;

namespace FirebaseDesktopHelper.Services
{
    public class FirebaseTokens
    {
        private CancellationTokenSource _cts;
        
        public string IdToken { get; private set; }
        public string RefreshToken { get; private set; }
        public static string Url => $"https://securetoken.googleapis.com/v1/token?key={FirebaseRestRequests.ApiKey}";

        public void SetIdAndRefreshToken(string idToken, string refreshToken, int expiredInSeconds)
        {
            RefreshToken = refreshToken;
            
            SetIdToken(idToken, expiredInSeconds);
        }

        public void SetIdToken(string idToken, int expiredInSeconds = 3600)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            IdToken = idToken;
            Task.Run(async () => await WaitForRefreshToken(_cts.Token, expiredInSeconds))
                .ConfigureAwait(true);
        }

        private async Task WaitForRefreshToken(CancellationToken ct, int expiredInSeconds = 3600)
        {
            expiredInSeconds = Mathf.RoundToInt(expiredInSeconds * 0.8f);
            
            try
            {
                await Task.Delay(expiredInSeconds * 1000, cancellationToken: ct);
                await Refresh();
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        public async Task Refresh()
        {
            var requestData = new RefreshTokenRequest(RefreshToken);
            string json = JsonConvert.SerializeObject(requestData);

            using var request = UnityWebRequest.Post(Url, json, "application/json");
            
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Refresh token failed: {request.responseCode} {request.result} {request.error}");
                return;
            }

            var response = JsonConvert.DeserializeObject<RefreshTokenResponse>(request.downloadHandler.text);
            
            if (!int.TryParse(response.expires_in, out int expiresInSeconds))
                expiresInSeconds = 3600;
            
            RefreshToken = response.refresh_token;
            SetIdToken(response.id_token, expiresInSeconds);
        }
    }
}