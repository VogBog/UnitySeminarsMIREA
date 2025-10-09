using System.Globalization;
using Newtonsoft.Json;

namespace Leaderboard.FirebaseDesktopHelper
{
    public struct FirebaseRestQuery
    {
        private string _path;
        private string _query;
        private string _json;
        private bool _hasChildren;

        public string Result => _path + _query;
        public string Json => _json;
        
        public FirebaseRestQuery(string url, string apiKey)
        {
            _path = url;
            _query = $".json?auth={apiKey}";
            _json = string.Empty;
            _hasChildren = false;

            if (!_path.EndsWith("/"))
                _path += "/";
        }
        
        public FirebaseRestQueryCompleter Call() => new(this);

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
            _path += _hasChildren ? $"/{childName}" : childName;
            _hasChildren = true;
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