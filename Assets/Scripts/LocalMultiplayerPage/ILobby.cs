using System;

namespace LocalMultiplayerPage
{
    public interface ILobby
    {
        public event Action<int> PlayersCountChanged;
        public string GetRoomName();
    }
}