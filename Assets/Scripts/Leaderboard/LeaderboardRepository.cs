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

        public static void GetNeighbours(
            PlayerAccount account,
            Action<List<PlayerAccount>, UnityWebRequest.Result> callback)
        {
            GetAccounts()
                .OrderBy(nameof(PlayerAccount.MaxScore))
                .StartAt(account.MaxScore)
                .LimitToFirst(2)
                .Call()
                .Get()
                .Void()
                .ObjectWrapped<ProjectRootWrapper>(nameof(ProjectRootWrapper.Accounts), (wrapper1, result1) =>
                {
                    if (result1 != UnityWebRequest.Result.Success)
                    {
                        callback?.Invoke(new List<PlayerAccount>(), result1);
                        return;
                    }
                    
                    GetAccounts()
                        .OrderBy(nameof(PlayerAccount.MaxScore))
                        .EndAt(account.MaxScore)
                        .LimitToLast(2)
                        .Call()
                        .Get()
                        .Void()
                        .ObjectWrapped<ProjectRootWrapper>(nameof(ProjectRootWrapper.Accounts), (wrapper2, result2) =>
                        {
                            if (result2 != UnityWebRequest.Result.Success)
                            {
                                callback?.Invoke(new List<PlayerAccount>(), result2);
                                return;
                            }

                            var list = wrapper1.Accounts
                                .Select(x => x.Value)
                                .Where(x => x.Id != account.Id)
                                .Take(1)
                                .ToList();
                            
                            list.AddRange(
                                wrapper2.Accounts
                                    .Select(x => x.Value)
                                    .Where(x => x.Id != account.Id)
                                    .Take(1)
                                    .ToList());
                            
                            list.Add(account);

                            list = list
                                .OrderByDescending(x => x.MaxScore)
                                .ToList();
                            
                            callback?.Invoke(list, result2);
                        });
                });
        }

        public static void GetLeaderboardSeveralTries(
            Action<List<PlayerAccount>> callback)
        {
            GetLeaderboard((list, res1) =>
            {
                if (res1 != UnityWebRequest.Result.ConnectionError)
                {
                    callback?.Invoke(list);
                    return;
                }
                
                GetLeaderboard((list2, res2) =>
                {
                    if (res2 != UnityWebRequest.Result.ConnectionError)
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

        public static void GetNeighboursSeveralTries(
            PlayerAccount account,
            Action<List<PlayerAccount>> callback)
        {
            GetNeighbours(account, (list1, res1) =>
            {
                if (res1 != UnityWebRequest.Result.ConnectionError)
                {
                    callback?.Invoke(list1);
                    return;
                }
                
                GetNeighbours(account, (list2, res2) =>
                {
                    if (res2 != UnityWebRequest.Result.ConnectionError)
                    {
                        callback?.Invoke(list2);
                        return;
                    }
                    
                    GetNeighbours(account, (list3, _) =>
                    {
                        callback?.Invoke(list3);
                    });
                });
            });
        }
    }
}