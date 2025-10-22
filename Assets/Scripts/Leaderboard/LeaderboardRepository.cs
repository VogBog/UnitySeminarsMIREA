using System;
using System.Collections.Generic;
using System.Linq;
using Account;
using FirebaseDesktopHelper;
using UnityEngine.Networking;

namespace Leaderboard
{
    public static class LeaderboardRepository
    {
        public const int LeaderboardRecordsCount = 10;
        
        public static FirebaseRestQuery GetAccounts() => AccountRepository.GetAccounts();
        
        public static void GetLeaderboard(Action<List<PlayerAccount>, UnityWebRequest.Result> callback)
        {
            GetAccounts()
                .OrderBy(nameof(PlayerAccount.MaxScore))
                .LimitToLast(LeaderboardRecordsCount)
                .Call()
                .Get()
                .Void()
                .ObjectWrapped<ProjectRootWrapper>(nameof(ProjectRootWrapper.Accounts), (wrapper, result) =>
                {
                    if (result != UnityWebRequest.Result.Success)
                    {
                        callback?.Invoke(new List<PlayerAccount>(), result);
                        return;
                    }

                    var list = wrapper.Accounts
                        .Select(x => x.Value)
                        .OrderByDescending(x => x.MaxScore)
                        .ToList();
                    
                    callback?.Invoke(list, result);
                });
        }

        public static void GetLeaderboardSeveralTries(
            Action<List<PlayerAccount>> callback)
        {
            GetLeaderboard((list, res1) =>
            {
                if (res1 == UnityWebRequest.Result.ConnectionError)
                {
                    callback?.Invoke(list);
                    return;
                }
                
                GetLeaderboard((list2, res2) =>
                {
                    if (res2 == UnityWebRequest.Result.ConnectionError)
                    {
                        callback?.Invoke(list2);
                        return;
                    }
                    
                    GetLeaderboard((list3, _) =>
                    {
                        callback?.Invoke(list3);
                    });
                });
            });
        }
    }
}