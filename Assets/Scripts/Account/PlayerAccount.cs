using System;

namespace Account
{
    [Serializable]
    public struct PlayerAccount
    {
        public string Id;
        public string Name;
        public float MaxScore;

        public PlayerAccount(string id, string name, float maxScore = 0)
        {
            Id = id;
            Name = name;
            MaxScore = maxScore;
        }
    }
}