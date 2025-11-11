namespace MainMenu
{
    public readonly struct LocalMultiplayerConnectionData
    {
        public readonly bool IsHost;
        public readonly string Ip;

        public LocalMultiplayerConnectionData(bool isHost, string ip)
        {
            IsHost = isHost;
            Ip = ip;
        }
    }
}