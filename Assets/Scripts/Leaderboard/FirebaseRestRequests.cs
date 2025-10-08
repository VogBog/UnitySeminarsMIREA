using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Leaderboard
{
    public static class FirebaseRestRequests
    {
        private static string Url;
        private static string ApiKey;

        public static void SetData(string url, string apiKey)
        {
            Url = url;
            ApiKey = apiKey;
        }

        public static FirebaseRestQuery GetQuery() => new(Url, ApiKey);

        private static void LogError(string method, UnityWebRequest request)
        {
            Debug.LogError($"FirebaseRestRequests::{method} Error {request.error}{Environment.NewLine}{request.url}" +
                           $"{Environment.NewLine}{request.downloadHandler?.text}");
        }

        public static async Task PutData(string path, string jsonData)
        {
            string url = $"{Url}/{path}.json?auth={ApiKey}";

            using var request = UnityWebRequest.Post(url, jsonData, "application/json");
            request.method = "PUT";
            
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LogError("PostData", request);
            }
        }

        public static async Task<string> SendGetQuery(FirebaseRestQuery query)
        {
            string url = query.Result;
            
            using var request = UnityWebRequest.Get(url);
            
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                LogError("SendGetQuery", request);
                return string.Empty;
            }
            
            Debug.Log($"Returning JSON {request.downloadHandler.text} by url {url}");
            return request.downloadHandler.text;
        }
    }
}