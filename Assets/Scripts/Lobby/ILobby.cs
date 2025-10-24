using System;

namespace Lobby
{
    public interface ILobby
    {
        bool IsAllPlayersActive { get; }
        
        event Action<PlayerData> PlayerChanged;
        event Action<int> PlayerDisconnected; 
        event Action<PlayerData[], bool> CompanyStarted; 
        
        void OnInitializeLobby(Lobby lobby);
        void OnLeaveLobby();
        void OnDisposeLobby();
        void InvokeCompanyStarted(PlayerData[] players);
        void LoadScene(int sceneIndex, bool isHost);
    }
}