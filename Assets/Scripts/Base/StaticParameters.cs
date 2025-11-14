namespace Base
{
    public static class StaticParameters
    {
        public enum NetworkTypes
        {
            LocalMultiplayer,
            Photon
        };
        
        public static int PlayersCount = 0;
        public static NetworkTypes NetworkType = NetworkTypes.LocalMultiplayer;
    }
}