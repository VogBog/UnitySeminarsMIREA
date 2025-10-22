using System;
using FirebaseDesktopHelper;
using Global;
using UnityEngine.Networking;

namespace Account
{
    public static class AccountRepository
    {
        public static FirebaseRestQuery GetAccounts() =>
            FirebaseRestRequests.RealtimeDatabase.Query()
                .GetChild(FirebaseConfig.ProjectRoot)
                .GetChild(FirebaseConfig.AccountsRoot);
        
        public static void GetAccountById(string id, Action<PlayerAccount, UnityWebRequest.Result> callback)
        {
            GetAccounts()
                .GetChild(id)
                .Call()
                .Get()
                .Void()
                .Object(callback);
        }

        public static void SetAccount(PlayerAccount account, Action<UnityWebRequest.Result> callback)
        {
            GetAccounts()
                .GetChild(account.Id)
                .SetJson(account)
                .Call()
                .Put()
                .Void()
                .Request(callback);
        }

        public static void SetAccount(PlayerAccount account) => SetAccount(account, res => { });

        public static void Register(string email, string password, string username,
            Action<UnityWebRequest.Result> callback)
        {
            FirebaseRestRequests.Authentication
                .SignUp()
                .FromEmailPassword(email, password)
                .Void()
                .Response((resp, result) =>
                {
                    if (result != UnityWebRequest.Result.Success)
                    {
                        callback?.Invoke(result);
                        return;
                    }

                    var acc = new PlayerAccount(resp.LocalId, username);
                    SetAccount(acc, callback);
                    StaticParameters.Account = acc;
                });
        }

        public static void LogIn(string email, string password, Action<UnityWebRequest.Result> callback)
        {
            FirebaseRestRequests.Authentication
                .SignIn()
                .FromEmailPassword(email, password)
                .Void()
                .Response((resp, result) =>
                {
                    if (result != UnityWebRequest.Result.Success)
                    {
                        callback?.Invoke(result);
                        return;
                    }
                    
                    GetAccountById(resp.LocalId, (acc, result2) =>
                    {
                        if (result2 != UnityWebRequest.Result.Success)
                        {
                            callback?.Invoke(result2);
                            return;
                        }

                        StaticParameters.Account = acc;
                        callback?.Invoke(result2);
                    });
                });
        }
    }
}