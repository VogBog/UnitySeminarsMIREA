using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace FirebaseDesktopHelper
{
    public struct FirebaseRestQuery
    {
        private readonly StringBuilder _path;
        private readonly StringBuilder _query;
        private string _json;
        private bool _hasChildren;

        public string Result => CreateResult();
        public string Json => _json;
        
        public FirebaseRestQuery(string url, string apiKey)
        {
            _path = new(url);

            _query = new(".json?auth=");
            _query.Append(apiKey);
            
            _json = string.Empty;
            _hasChildren = false;

            if (!url.EndsWith("/"))
                _path.Append("/");
        }

        public string CreateResult()
        {
            var sb = new StringBuilder();
            sb.Append(_path);
            sb.Append(_query);
            return sb.ToString();
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
            _path.Append(_hasChildren ? $"/{childName}" : childName);
            _hasChildren = true;
            return this;
        }

        public FirebaseRestQuery OrderByKey()
        {
            _query.Append("&orderBy=\"$key\"");
            return this;
        }
        
        public FirebaseRestQuery OrderByValue()
        {
            _query.Append("&orderBy=\"$value\"");
            return this;
        }
        
        public FirebaseRestQuery OrderBy(string key)
        {
            _query.Append($"&orderBy=\"{key}\"");
            return this;
        }

        public FirebaseRestQuery LimitToFirst(int count)
        {
            _query.Append($"&limitToFirst={count}");
            return this;
        }

        public FirebaseRestQuery LimitToLast(int count)
        {
            _query.Append($"&limitToLast={count}");
            return this;
        }

        public FirebaseRestQuery StartAt(double value)
        {
            _query.Append("&startAt=").Append(value.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
            return this;
        }

        public FirebaseRestQuery EndAt(double value)
        {
            _query.Append("&endAt=").Append(value.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
            return this;
        }
        
        public FirebaseRestQuery StartAt(string startFrom)
        {
            _query.Append("&startAt=\"").Append(startFrom).Append("\"");
            return this;
        }

        public FirebaseRestQuery EndAt(string endAt)
        {
            _query.Append("&endAt=\"").Append(endAt).Append("\"");
            return this;
        }
    }
}