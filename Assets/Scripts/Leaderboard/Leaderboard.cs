using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account;
using Data;
using FirebaseDesktopHelper;
using UnityEngine.Networking;

namespace Leaderboard
{
    public class Leaderboard
    {
        public readonly List<PlayerAccount> Leaderboard4 = new(MaxPlayers);
        public readonly List<PlayerAccount> Leaderboard2Vs2 = new(MaxPlayers);
        public readonly List<PlayerAccount> Leaderboard1Vs1 = new(MaxPlayers);

        public const int MaxPlayers = 10;

        public async Task<UnityWebRequest.Result> LoadAll()
        {
            var status = await SeveralTries(Load4());
            if (status != UnityWebRequest.Result.Success)
                return status;

            status = await SeveralTries(Load2Vs2());
            if (status != UnityWebRequest.Result.Success)
                return status;

            status = await SeveralTries(Load1Vs1());
            return status;
        }

        public async Task<UnityWebRequest.Result> SeveralTries(Task<UnityWebRequest.Result> task)
        {
            var result = UnityWebRequest.Result.ConnectionError;
            
            for (int i = 0; i < 3; i++)
            {
                result = await task;
                
                if(result == UnityWebRequest.Result.Success)
                    return result;

                if(i != 2)
                    await Task.Delay(3000);
            }

            return result;
        }

        public async Task<UnityWebRequest.Result> Load4()
        {
            var (result, status) = await GetQuery(nameof(PlayerAccount.Max4Score));
            return ApplyResults(result, status, Leaderboard4, acc => acc.Max4Score);
        }

        public async Task<UnityWebRequest.Result> Load2Vs2()
        {
            var (result, status) = await GetQuery(nameof(PlayerAccount.Max2V2Score));
            return ApplyResults(result, status, Leaderboard2Vs2, acc => acc.Max2V2Score);
        }

        public async Task<UnityWebRequest.Result> Load1Vs1()
        {
            var (result, status) = await GetQuery(nameof(PlayerAccount.Max1V1Score));
            return ApplyResults(result, status, Leaderboard1Vs1, acc => acc.Max1V1Score);
        }

        private Task<(ProjectRootData, UnityWebRequest.Result)> GetQuery(string orderBy)
            => FirebaseRestRequests.RealtimeDatabase.Query()
                .GetAccounts()
                .OrderBy(orderBy)
                .LimitToLast(MaxPlayers)
                .Call()
                .Get()
                .Async()
                .ObjectWrapped<ProjectRootData>(nameof(ProjectRootData.PlayerAccounts));

        private UnityWebRequest.Result ApplyResults(
            ProjectRootData results, UnityWebRequest.Result status,
            List<PlayerAccount> list, Func<PlayerAccount, float> orderBy)
        {
            if (status != UnityWebRequest.Result.Success)
                return status;
            if (list == null)
                return UnityWebRequest.Result.DataProcessingError;

            var accounts = results.PlayerAccounts.Values;
            if (accounts.Count == 0)
                return UnityWebRequest.Result.DataProcessingError;

            var orderedAccounts = accounts.OrderByDescending(orderBy);
            
            list.Clear();
            list.AddRange(orderedAccounts);

            return status;
        }
    }
}