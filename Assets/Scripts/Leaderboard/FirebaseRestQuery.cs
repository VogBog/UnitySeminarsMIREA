using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace Leaderboard
{
    public struct FirebaseRestQuery
    {
        private string _path;
        private string _query;

        public string Result => _path + _query;
        
        public FirebaseRestQuery(string url, string apiKey)
        {
            _path = url;
            _query = $".json?auth={apiKey}";
        }

        public Task<string> CompleteAsync()
        {
            return FirebaseRestRequests.SendGetQuery(this);
        }

        public async Task CompleteCallback(Action<string> onComplete)
        {
            var result = await FirebaseRestRequests.SendGetQuery(this);
            onComplete?.Invoke(result);
        }

        public IEnumerator CompleteCoroutine(Action<string> result)
        {
            var strRef = new Reference<string>();
            yield return CompleteCallback(str => strRef.Value = str);
            yield return new WaitWhile(() => string.IsNullOrEmpty(strRef.Value));
            
            result?.Invoke(strRef.Value);
        }

        public async Task<T> CompleteAsync<T>()
        {
            string result = await FirebaseRestRequests.SendGetQuery(this);
            return string.IsNullOrEmpty(result) ? default : JsonUtility.FromJson<T>(result);
        }

        public async Task CompleteCallback<T>(Action<T> onComplete)
        {
            var result = await CompleteAsync<T>();
            onComplete?.Invoke(result);
        }

        public IEnumerator CompleteCoroutine<T>(Action<T> result)
        {
            var strRef = new Reference<string>();
            yield return CompleteCallback(str => strRef.Value = str);
            yield return new WaitWhile(() => string.IsNullOrEmpty(strRef.Value));
            
            result?.Invoke(JsonUtility.FromJson<T>(strRef.Value));
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