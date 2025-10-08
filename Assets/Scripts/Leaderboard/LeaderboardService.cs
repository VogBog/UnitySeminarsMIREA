using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leaderboard
{
    public static class LeaderboardService
    {
        public class Reference<T>
        {
            public T Value;
        }

        public class ListRef
        {
            public List<LeaderboardRepository.PlayerData> List;
        }

        public const int NearPlayersCount = 1;
        public const int LeadersCount = 10;
        
        public static IEnumerator SaveRoutine(string playerName, float time)
        {
            var prevTimeRef = new Reference<float>
            {
                Value = -1f
            };

            yield return LeaderboardRepository.GetWithCallback(playerName, t => prevTimeRef.Value = t.Time);
            yield return new WaitWhile(() => !Mathf.Approximately(prevTimeRef.Value, -1));
            
            float prevTime = prevTimeRef.Value;
            if (time < prevTime)
                yield break;

            yield return LeaderboardRepository.Put(playerName, time);
        }

        public static IEnumerator GetLeadersRoutine(Action<List<LeaderboardRepository.PlayerData>> callback)
        {
            var listRef = new ListRef();
            yield return LeaderboardRepository.GetLeadersWithCallback(LeadersCount, list => listRef.List = list);
            yield return new WaitWhile(() => listRef.List == null);
            callback?.Invoke(listRef.List);
        }

        public static IEnumerator GetNearPlayersRoutine(string playerName, Action<List<LeaderboardRepository.PlayerData>> callback)
        {
            var listRef = new ListRef();
            yield return LeaderboardRepository.GetNearPlayerWithCallback(playerName, NearPlayersCount,
                list => listRef.List = list);
            yield return new WaitWhile(() => listRef.List == null);
            callback?.Invoke(listRef.List);
        }

        public static IEnumerator GetTotalLeaderboard(IEnumerable<string> playerNames, Action<List<LeaderboardRepository.PlayerData>> callback)
        {
            var leadersRef = new ListRef();
            yield return LeaderboardRepository.GetLeadersWithCallback(LeadersCount, list => leadersRef.List = list);
            yield return new WaitWhile(() => leadersRef.List == null);
            
            var leaders = leadersRef.List;

            var showNear = new List<string>();
            foreach (var playerName in playerNames)
            {
                if(leaders.FindIndex(data => data.Name == playerName) == -1)
                    showNear.Add(playerName);
            }

            foreach (var showNearName in showNear)
            {
                leaders.Add(new("...", 0f));

                var nearRef = new ListRef();
                yield return GetNearPlayersRoutine(showNearName, list => nearRef.List = list);
                var nearList = nearRef.List;
                
                if(nearList == null || nearList.Count == 0)
                    continue;

                leaders.AddRange(nearList);
            }
            
            callback?.Invoke(leaders);
        }
    }
}