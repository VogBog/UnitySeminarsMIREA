using System;
using System.Threading.Tasks;
using FirebaseDesktopHelper.Configs;
using FirebaseDesktopHelper.Services;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper
{
    public static class FirebaseRestRequests
    {
        private static FirebaseRealtimeDatabase _realtimeDatabase;
        private static FirebaseAuthentication _authentication;
        private static FirebaseTokens _tokens;
        
        public static string RealtimeDatabaseUrl { get; private set; }
        public static string ApiKey { get; private set; }
        public static FirebaseRealtimeDatabase RealtimeDatabase => _realtimeDatabase ??= new FirebaseRealtimeDatabase();
        public static FirebaseAuthentication Authentication => _authentication ??= new FirebaseAuthentication();
        public static FirebaseTokens Tokens => _tokens ??= new FirebaseTokens();
        
        public static void SetData(IFirebaseConfig config) => SetData(config.RealtimeDatabaseUrl, config.ApiKey);

        public static void SetData(string url, string apiKey)
        {
            RealtimeDatabaseUrl = url;
            ApiKey = apiKey;
        }
        
        public static async Task<(string, UnityWebRequest.Result)> SendRequest(
            UnityWebRequest request, string methodName, int attempts = 0)
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                if (request.responseCode == 401 && !string.IsNullOrEmpty(Tokens.RefreshToken) &&
                    attempts == 0)
                {
                    Debug.Log("Auto refresh token");
                    await Tokens.Refresh();
                }
                
                LogError(methodName, request);
                return (request.result.ToString(), request.result);
            }
            
            Debug.Log($"Returning JSON {request.downloadHandler.text} by url {request.url}");
            return (request.downloadHandler.text, request.result);
        }

        public static void LogError(string method, UnityWebRequest request)
        {
            Debug.LogError($"FirebaseRestRequests::{method} Error {request.error}{Environment.NewLine}{request.url}" +
                           $"{Environment.NewLine}{request.downloadHandler?.text}");
        }
    }
}