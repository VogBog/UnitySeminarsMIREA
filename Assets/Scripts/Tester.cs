using System;
using System.Collections;
using System.Collections.Generic;
using Leaderboard.FirebaseDesktopHelper;
using UnityEngine;

namespace Global
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private Config _config;

        private void Awake()
        {
            FirebaseRestRequests.SetData(_config.FirebaseUrl, _config.FirebaseApiKey);
        }

        public void StartPutTest() => StartCoroutine(TestRoutine(PutTest(), "PUT"));
        public void StartGetTest() => StartCoroutine(TestRoutine(GetTest(), "GET"));
        public void StartGetObjectTest() => StartCoroutine(TestRoutine(GetObjectTest(), "GET Object"));
        public void StartDeleteTest() => StartCoroutine(TestRoutine(DeleteTest(), "DELETE"));

        private IEnumerator TestRoutine(IEnumerator enumerator, string testName)
        {
            Debug.Log($"Starting test {testName}");
            yield return enumerator;
            Debug.Log($"Test {testName} completed!");
        }

        private IEnumerator PutTest()
        {
            var data = new PlayerData()
            {
                Name = "TestName",
                Time = 500f
            };

            yield return FirebaseRestRequests.GetQuery()
                .GetChild("Records")
                .GetChild(data.Name)
                .SetJson(data)
                .Call()
                    .Put()
                    .Coroutine()
                    .Empty();
        }

        private IEnumerator GetTest()
        {
            yield return FirebaseRestRequests.GetQuery()
                .GetChild("Records")
                .OrderBy("Time")
                .StartAt(200f)
                .LimitToFirst(3)
                .Call()
                    .Get()
                    .Coroutine()
                    .String(Debug.Log);
        }

        private IEnumerator GetObjectTest()
        {
            var resRef = new ReferenceWithFlag<Players>();

            yield return FirebaseRestRequests.GetQuery()
                .GetChild("Records")
                .Call()
                .Get()
                .Coroutine()
                .ObjectWrapped<Players>("Records", resRef.Set);
            yield return new WaitUntil(() => resRef.IsReady);

            foreach (var kvp in resRef.Value.Records)
            {
                Debug.Log($"Name: {kvp.Value.Name}  ---  Time: {kvp.Value.Time}");
            }
        }

        private IEnumerator DeleteTest()
        {
            yield return FirebaseRestRequests.GetQuery()
                .GetChild("Records")
                .GetChild("TestName")
                .Call()
                    .Delete()
                    .Coroutine()
                    .Empty();
        }

        [Serializable]
        public struct PlayerData
        {
            public string Name;
            public float Time;
        }

        [Serializable]
        public struct Players
        {
            public Dictionary<string, PlayerData> Records;
        }
    }
}