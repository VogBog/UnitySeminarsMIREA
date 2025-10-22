using System;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    public struct FirebaseSignUpVoid
    {
        private readonly FirebaseSignUpAsync _method;

        public FirebaseSignUpVoid(FirebaseSignUpAsync method)
        {
            _method = method;
        }

        public async void Empty()
        {
            try
            {
                await _method.Empty();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void Result(Action<UnityWebRequest.Result> onComplete)
        {
            try
            {
                var result = await _method.Empty();
                onComplete?.Invoke(result);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void Json(Action<string, UnityWebRequest.Result> onComplete)
        {
            try
            {
                var (str, res) = await _method.Json();
                onComplete?.Invoke(str, res);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void Response(Action<SignUpResponse, UnityWebRequest.Result> onComplete)
        {
            try
            {
                var (resp, res) = await _method.Response();
                onComplete?.Invoke(resp, res);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}