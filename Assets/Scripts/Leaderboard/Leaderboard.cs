using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account;
using Data;
using FirebaseDesktopHelper;
using UnityEngine;
using UnityEngine.Networking;

namespace Leaderboard
{
    public class Leaderboard
    {
        public readonly List<PlayerAccount> Leaderboard4 = new(MaxPlayers);
        public readonly List<PlayerAccount> Leaderboard2Vs2 = new(MaxPlayers);
        public readonly List<PlayerAccount> Leaderboard1Vs1 = new(MaxPlayers);

        public readonly List<PlayerAccount> ActivePlayer4 = new(NearPlayersCount * 2 + 1);
        public readonly List<PlayerAccount> ActivePlayer2Vs2 = new(NearPlayersCount * 2 + 1);
        public readonly List<PlayerAccount> ActivePlayer1Vs1 = new(NearPlayersCount * 2 + 1);

        public const int MaxPlayers = 10;
        public const int NearPlayersCount = 1;

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

        public Task<UnityWebRequest.Result> Load4()
            => Facade(nameof(PlayerAccount.Max4Score), acc => acc.Max4Score,
                () => Leaderboard4, () => ActivePlayer4);

        public Task<UnityWebRequest.Result> Load2Vs2()
            => Facade(nameof(PlayerAccount.Max2V2Score), acc => acc.Max2V2Score,
                () => Leaderboard2Vs2, () => ActivePlayer2Vs2);

        public Task<UnityWebRequest.Result> Load1Vs1()
            => Facade(nameof(PlayerAccount.Max1V1Score), acc => acc.Max1V1Score,
                () => Leaderboard1Vs1, () => ActivePlayer1Vs1);

        private Task<(ProjectRootData, UnityWebRequest.Result)> GetQuery(string orderBy)
            => FirebaseRestRequests.RealtimeDatabase.Query()
                .GetAccounts()
                .OrderBy(orderBy)
                .LimitToLast(MaxPlayers)
                .Call()
                .Get()
                .Async()
                .ObjectWrapped<ProjectRootData>(nameof(ProjectRootData.PlayerAccounts));

        private async Task<(List<PlayerAccount>, UnityWebRequest.Result)> GetQueryForPlayer(
            string orderBy, PlayerAccount playerAccount, float score)
        {
            var result = new List<PlayerAccount>();

            var (root1, status1) = await FirebaseRestRequests.RealtimeDatabase.Query()
                .GetAccounts()
                .OrderBy(orderBy)
                .StartAt(score)
                .LimitToFirst(NearPlayersCount + 1)
                .Call()
                .Get()
                .Async()
                .ObjectWrapped<ProjectRootData>(nameof(ProjectRootData.PlayerAccounts));

            if (status1 != UnityWebRequest.Result.Success)
                return (null, status1);

            var (root2, status2) = await FirebaseRestRequests.RealtimeDatabase.Query()
                .GetAccounts()
                .OrderBy(orderBy)
                .EndAt(score)
                .LimitToLast(NearPlayersCount + 1)
                .Call()
                .Get()
                .Async()
                .ObjectWrapped<ProjectRootData>(nameof(ProjectRootData.PlayerAccounts));

            if (status2 != UnityWebRequest.Result.Success)
                return (null, status2);

            string playerId = playerAccount.Id;
            if (root1.PlayerAccounts != null)
            {
                result.AddRange(root1.PlayerAccounts.Values.Where(acc => acc.Id != playerId));
            }
            
            result.Add(playerAccount);

            if (root2.PlayerAccounts != null)
            {
                result.AddRange(root2.PlayerAccounts.Values.Where(acc => acc.Id != playerId));
            }

            return (result, UnityWebRequest.Result.Success);
        }

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

        private async Task<UnityWebRequest.Result> Facade(
            string orderBy, Func<PlayerAccount, float> getScore, Func<List<PlayerAccount>> getList,
            Func<List<PlayerAccount>> getListNearPlayers)
        {
            var (result1, status1) = await GetQuery(orderBy);
            var status11 = ApplyResults(result1, status1, getList.Invoke(), getScore);
            if(status11 != UnityWebRequest.Result.Success)
                return status11;

            var playerAcc = StaticParameters.PlayerAccount;
            if (StaticParameters.SinglePlayer || string.IsNullOrEmpty(playerAcc.Id))
                return status11;

            var list = getList.Invoke();
            if (list == null)
                return UnityWebRequest.Result.DataProcessingError;
            
            if (list.Exists(acc => acc.Id == playerAcc.Id))
                return UnityWebRequest.Result.Success;
            
            var (result2, status2) =
                await GetQueryForPlayer(orderBy, playerAcc, getScore.Invoke(playerAcc));
            if (status2 != UnityWebRequest.Result.Success)
                return status2;
            
            var listNearPlayers = getListNearPlayers.Invoke();
            if (listNearPlayers == null)
                return UnityWebRequest.Result.DataProcessingError;
            
            listNearPlayers.Clear();
            listNearPlayers.AddRange(result2);

            return UnityWebRequest.Result.Success;
        }
    }
}