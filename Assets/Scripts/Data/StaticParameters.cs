using System.Collections.Generic;
using Account;
using Lobby;
using MainMenu;

namespace Data
{
    public static class StaticParameters
    {
        public static GameType GameType;
        public static NetworkTypes NetworkType;
        public static PlayerData[] Players;
        public static List<(string, float)> PlayerScore;
        public static PlayerAccount PlayerAccount;
        public static bool SinglePlayer = true;
    }
}