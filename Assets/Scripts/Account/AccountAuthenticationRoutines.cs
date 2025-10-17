using System;
using System.Collections;
using System.Collections.Generic;
using FirebaseDesktopHelper;
using FirebaseDesktopHelper.Services.Auth.SignIn;
using FirebaseDesktopHelper.Services.Auth.SignUp;
using UnityEngine;
using UnityEngine.Networking;

namespace Account
{
    public static class AccountAuthenticationRoutines
    {
        public const string PlayerPrefsEmail = "Email";
        public const string PlayerPrefsPassword = "Password";
        public const string ProjectRoot = "ElementalsRoot";
        public const string FirebaseAccounts = "Accounts";

        [Serializable]
        public struct ProjectRootData
        {
            public Dictionary<string, PlayerAccount> PlayerAccounts;
        }

        public static IEnumerator SeveralTries(IEnumerator enumerator, Func<UnityWebRequest.Result> getResult)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return enumerator;

                if (getResult.Invoke() == UnityWebRequest.Result.Success)
                    yield break;

                yield return new WaitForSeconds(3f);
            }
        }
        
        public static IEnumerator TrySignInRoutine(Action<bool, SignInResponse, UnityWebRequest.Result> onComplete)
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsEmail) || !PlayerPrefs.HasKey(PlayerPrefsPassword))
            {
                onComplete?.Invoke(false, default, UnityWebRequest.Result.Success);
                yield break;
            }
            
            string email = PlayerPrefs.GetString(PlayerPrefsEmail);
            string password = PlayerPrefs.GetString(PlayerPrefsPassword);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                onComplete?.Invoke(false, default, UnityWebRequest.Result.Success);
                yield break;
            }

            yield return TrySignInRoutine(email, password, onComplete);
        }

        public static IEnumerator TrySignInRoutine(string email, string password,
            Action<bool, SignInResponse, UnityWebRequest.Result> onComplete)
        {
            var responseRef = new Reference<(SignInResponse, UnityWebRequest.Result)>();
            yield return FirebaseRestRequests.Authentication
                .SignIn()
                .FromEmailPassword(email, password)
                .Coroutine()
                .Response((resp, status) => responseRef.Value = (resp, status));

            bool isSuccess =
                responseRef.Value.Item2 == UnityWebRequest.Result.Success;

            if (isSuccess)
            {
                PlayerPrefs.SetString(PlayerPrefsEmail, email);
                PlayerPrefs.SetString(PlayerPrefsPassword, password);
            }
            
            onComplete?.Invoke(isSuccess, responseRef.Value.Item1, responseRef.Value.Item2);
        }

        public static IEnumerator TrySignUpRoutine(
            string email, string password, string nickName,
            Action<bool, SignUpResponse, UnityWebRequest.Result> onComplete)
        {
            var isFreeRef = new Reference<int>();
            yield return IsNickNameFreeRoutine(nickName,
                b => isFreeRef.Value = b ? 1 : 0,
                () => isFreeRef.Value = -1);

            if (isFreeRef.Value != 1)
            {
                onComplete?.Invoke(false, default, isFreeRef.Value == 0 ?
                        UnityWebRequest.Result.Success :
                        UnityWebRequest.Result.ConnectionError);
                yield break;
            }
            
            var responseRef = new Reference<(SignUpResponse, UnityWebRequest.Result)>();
            yield return FirebaseRestRequests.Authentication
                .SignUp()
                .FromEmailPassword(email, password)
                .Coroutine()
                .Response((resp, status) => responseRef.Value = (resp, status));

            if (responseRef.Value.Item2 != UnityWebRequest.Result.Success ||
                string.IsNullOrEmpty(responseRef.Value.Item1.IdToken))
            {
                onComplete?.Invoke(false, default, responseRef.Value.Item2);
                yield break;
            }

            var accountToRegister = new PlayerAccount()
            {
                NickName = nickName,
                MaxScore = 0f
            };

            var strRef = new Reference<(string, UnityWebRequest.Result)>();

            while (true)
            {
                yield return FirebaseRestRequests.RealtimeDatabase
                    .Query()
                    .GetChild(ProjectRoot)
                    .GetChild(FirebaseAccounts)
                    .GetChild(responseRef.Value.Item1.LocalId)
                    .SetJson(accountToRegister)
                    .Call()
                    .Put()
                    .Coroutine()
                    .String((str, status) => strRef.Value = (str, status));

                if (strRef.Value.Item2 != UnityWebRequest.Result.Success)
                    yield return new WaitForSeconds(3f);
                else
                    break;
            }
            
            onComplete?.Invoke(!string.IsNullOrEmpty(strRef.Value.Item1), responseRef.Value.Item1, strRef.Value.Item2);
        }

        public static IEnumerator GetPlayerRoutine(string uuid,
            Action<PlayerAccount, UnityWebRequest.Result> onComplete)
        {
            yield return FirebaseRestRequests.RealtimeDatabase.Query()
                .GetChild(ProjectRoot)
                .GetChild(FirebaseAccounts)
                .GetChild(uuid)
                .Call()
                .Get()
                .Coroutine()
                .Object(onComplete);
        }

        public static IEnumerator IsNickNameFreeRoutineSeveralTries(
            string nickName, Action<bool> isFree, Action onError)
        {
            for (int i = 0; i < 5; i++)
            {
                var intRef = new Reference<int>();
                yield return IsNickNameFreeRoutine(
                    nickName,
                    b => intRef.Value = b ? 1 : 0,
                    () => intRef.Value = -1);

                if (intRef.Value != -1)
                {
                    isFree?.Invoke(intRef.Value == 1);
                    yield break;
                }

                yield return new WaitForSeconds(3f);
            }
            
            onError?.Invoke();
        }

        public static IEnumerator IsNickNameFreeRoutine(string nickName, Action<bool> isFree, Action onError)
        {
            var strRef = new Reference<(ProjectRootData, UnityWebRequest.Result)>();
            yield return FirebaseRestRequests.RealtimeDatabase.Query()
                .GetChild(ProjectRoot)
                .GetChild(FirebaseAccounts)
                .Call()
                .Get()
                .Coroutine()
                .ObjectWrapped<ProjectRootData>(nameof(ProjectRootData.PlayerAccounts),
                    (data, status) => strRef.Value = (data, status));

            var response = strRef.Value.Item1;
            var status = strRef.Value.Item2;

            if (status != UnityWebRequest.Result.Success)
            {
                onError?.Invoke();
                yield break;
            }

            if (response.PlayerAccounts == null)
            {
                isFree?.Invoke(true);
                yield break;
            }

            foreach (var kvp in response.PlayerAccounts)
            {
                if (kvp.Value.NickName == nickName)
                {
                    isFree?.Invoke(false);
                    yield break;
                }
            }
            
            isFree?.Invoke(true);
        }
    }
}