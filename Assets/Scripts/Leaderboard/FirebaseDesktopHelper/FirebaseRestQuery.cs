using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Leaderboard.FirebaseDesktopHelper
{
    public struct FirebaseRestQuery
    {
        private string _path;
        private string _query;
        private string _json;

        public string Result => _path + _query;
        public string Json => _json;
        
        public FirebaseRestQuery(string url, string apiKey)
        {
            _path = url;
            _query = $".json?auth={apiKey}";
            _json = string.Empty;
        }

        public Task<string> GetAsync()
        {
            return FirebaseRestRequests.SendGetQuery(this);
        }

        public async Task GetCallback(Action<string> onComplete)
        {
            var result = await FirebaseRestRequests.SendGetQuery(this);
            onComplete?.Invoke(result);
        }

        public IEnumerator GetCoroutine(Action<string> result)
        {
            var strRef = new ReferenceWithFlag<string>();
            yield return GetCallback(str => strRef.Set(str));
            yield return new WaitUntil(() => strRef.IsReady);
            
            result?.Invoke(strRef.Value);
        }

        public async Task<T> GetAsync<T>()
        {
            string result = await FirebaseRestRequests.SendGetQuery(this);
            return string.IsNullOrEmpty(result) ? default : JsonUtility.FromJson<T>(result);
        }

        public async Task GetCallback<T>(Action<T> onComplete)
        {
            var result = await GetAsync<T>();
            onComplete?.Invoke(result);
        }

        public IEnumerator GetCoroutine<T>(Action<T> result)
        {
            var strRef = new ReferenceWithFlag<string>();
            yield return GetCallback(str => strRef.Set(str));
            yield return new WaitUntil(() => strRef.IsReady);
            
            result?.Invoke(JsonConvert.DeserializeObject<T>(strRef.Value));
        }

        public FirebaseRestQuery SetJson(string json)
        {
            _json = json;
            return this;
        }

        public FirebaseRestQuery SetJson<T>(T obj)
        {
            _json = JsonConvert.SerializeObject(obj);
            return this;
        }

        public FirebaseRestQuery GetChild(string childName)
        {
            _path += $"/{childName}";
            return this;
        }

        public FirebaseRestQuery OrderByKey()
        {
            _query += "&orderBy=\"$key\"";
            return this;
        }
        
        public FirebaseRestQuery OrderByValue()
        {
            _query += "&orderBy=\"$value\"";
            return this;
        }
        
        public FirebaseRestQuery OrderBy(string key)
        {
            _query += $"&orderBy=\"{key}\"";
            return this;
        }

        public FirebaseRestQuery LimitToFirst(int count)
        {
            _query += $"&limitToFirst={count}";
            return this;
        }

        public FirebaseRestQuery LimitToLast(int count)
        {
            _query += $"&limitToLast={count}";
            return this;
        }

        public FirebaseRestQuery StartAt(double value)
        {
            _query += $"&startAt={value.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}";
            return this;
        }

        public FirebaseRestQuery EndAt(double value)
        {
            _query += $"&endAt={value.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}";
            return this;
        }
        
        public FirebaseRestQuery StartAt(string startFrom)
        {
            _query += $"&startAt=\"{startFrom}\"";
            return this;
        }

        public FirebaseRestQuery EndAt(string endAt)
        {
            _query += $"&endAt=\"{endAt}\"";
            return this;
        }
    }
}