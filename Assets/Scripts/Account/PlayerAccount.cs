using System;

namespace Account
{
    [Serializable]
    public struct PlayerAccount
    {
        public string Id;
        public string NickName;
        public float Max4Score;
        public float Max2V2Score;
        public float Max1V1Score;

        public PlayerAccount(string id, string nickName)
        {
            Id = id;
            NickName = nickName;
            Max4Score = 0f;
            Max2V2Score = 0f;
            Max1V1Score = 0f;
        }
    }
}