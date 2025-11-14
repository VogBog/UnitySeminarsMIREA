namespace Global
{
    public static class StaticParameters
    {
        public readonly struct PlayerFinishData
        {
            public readonly int PlayerIndex;
            public readonly float Time;

            public PlayerFinishData(int playerIndex, float time)
            {
                PlayerIndex = playerIndex;
                Time = time;
            }
        }

        public enum NetworkTypes
        {
            Single,
            LocalMultiplayer
        };
        
        public static int PlayersCount = 1;
        public static PlayerFinishData[] FinishData = null;
        public static NetworkTypes NetworkType = NetworkTypes.Single;
    }
}