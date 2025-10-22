using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    public readonly struct FirebaseSignInCoroutine
    {
        private readonly string _json;

        public FirebaseSignInCoroutine(string json)
        {
            _json = json;
        }
        
        public IEnumerator Empty()
        {
            var boolRef = new Reference<bool>();
            var callbackHandler = new FirebaseSignInCallback(_json);

            yield return callbackHandler.Empty(_ => boolRef.Value = true);
            yield return new WaitUntil(() => boolRef.Value);
        }

        public IEnumerator Status(Action<UnityWebRequest.Result> getResultStatus)
        {
            var statusRef = new ReferenceWithFlag<UnityWebRequest.Result>();
            var callbackHandler = new FirebaseSignInCallback(_json);

            yield return callbackHandler.Empty(statusRef.Set);
            yield return new WaitUntil(() => statusRef.IsReady);
            
            getResultStatus?.Invoke(statusRef.Value);
        }

        public IEnumerator Json(Action<string, UnityWebRequest.Result> getJson)
        {
            var jsonRef = new ReferenceWithFlag<(string, UnityWebRequest.Result)>();
            var callbackHandler = new FirebaseSignInCallback(_json);

            yield return callbackHandler.Json((str, status) => jsonRef.Set((str, status)));
            yield return new WaitUntil(() => jsonRef.IsReady);
            
            getJson?.Invoke(jsonRef.Value.Item1, jsonRef.Value.Item2);
        }

        public IEnumerator Response(Action<SignInResponse, UnityWebRequest.Result> getResponse)
        {
            var responseRef = new ReferenceWithFlag<(SignInResponse, UnityWebRequest.Result)>();
            var callbackHandler = new FirebaseSignInCallback(_json);

            yield return callbackHandler.Response((response, status) => responseRef.Set((response, status)));
            yield return new WaitUntil(() => responseRef.IsReady);
            
            getResponse?.Invoke(responseRef.Value.Item1, responseRef.Value.Item2);
        }
    }
}