using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Leaderboard
{
    public static class LeaderboardRepository
    {
        [Serializable]
        public struct PlayerData
        {
            public string Name;
            public float Time;

            public PlayerData(string name, float time)
            {
                Name = name;
                Time = time;
            }
        }

        [Serializable]
        public struct Players
        {
            public Dictionary<string, PlayerData> Records;

            public static Players FromJson(string json)
            {
                string totalJson = $"{{\"Records\": {json}}}";
                return JsonConvert.DeserializeObject<Players>(totalJson);
            }
        }

        public const string Path = "Records";
        
        public static Task Put(string playerName, float time)
        {
            string json = JsonUtility.ToJson(new PlayerData(playerName, time), true);
            Debug.Log($"Post json: {json}");
            return FirebaseRestRequests.PutData(
                $"{Path}/{playerName}",
                json);
        }

        public static async Task<PlayerData> Get(string playerName)
        {
            var query = FirebaseRestRequests.GetQuery().GetChild(Path).GetChild(playerName);
            string json = await FirebaseRestRequests.SendGetQuery(query);
            if (string.IsNullOrEmpty(json))
                return default;

            var obj = JsonUtility.FromJson<PlayerData>(json);
            return obj;
        }

        public static async Task GetWithCallback(string playerName, Action<PlayerData> callback)
        {
            var obj = await Get(playerName);
            callback?.Invoke(obj);
        }

        public static async Task<List<PlayerData>> GetLeaders(int playersCount)
        {
            var json = await FirebaseRestRequests.GetQuery().GetChild(Path)
                .OrderBy("Time").LimitToFirst(playersCount).CompleteAsync();

            return Players.FromJson(json).Records.Select(kvp => kvp.Value).OrderBy(kvp => kvp.Time).ToList();
        }

        public static async Task GetLeadersWithCallback(int playersCount, Action<List<PlayerData>> callback)
        {
            var result = await GetLeaders(playersCount);
            callback?.Invoke(result);
        }

        public static async Task<List<PlayerData>> GetNearPlayer(string playerName, int playersCount)
        {
            var result = new List<PlayerData>();
            
            var data = await Get(playerName);
            if(data.Equals(default(PlayerData)))
                return result;

            var snapshotBeforeJson = await FirebaseRestRequests.GetQuery().GetChild(Path)
                .OrderBy("Time").EndAt(data.Time).LimitToLast(playersCount).CompleteAsync();

            var snapshotAfterJson = await FirebaseRestRequests.GetQuery().GetChild(Path)
                .OrderBy("Time").StartAt(data.Time).LimitToFirst(playersCount + 1).CompleteAsync();
            
            var snapshotBefore = Players.FromJson(snapshotBeforeJson);
            var snapshotAfter = Players.FromJson(snapshotAfterJson);

            if(snapshotBefore.Records != null)
                result.AddRange(snapshotBefore.Records.Values.Where(before => before.Name != playerName));
            if(snapshotAfter.Records != null)
                result.AddRange(snapshotAfter.Records.Values.Where(after => after.Name != playerName));
            
            result.Add(data);

            return result.OrderBy(x => x.Time).ToList();
        }

        public static async Task GetNearPlayerWithCallback(string playerName, int playersCount, 
            Action<List<PlayerData>> callback)
        {
            var result = await GetNearPlayer(playerName, playersCount);
            callback?.Invoke(result);
        }
    }
}