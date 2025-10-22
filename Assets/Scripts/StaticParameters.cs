using Account;

namespace Global
{
    public static class StaticParameters
    {
        public static int PlayersCount = 1;
        public static float PlayerScore = 0;
        public static PlayerAccount Account = new(string.Empty, string.Empty);
    }
}