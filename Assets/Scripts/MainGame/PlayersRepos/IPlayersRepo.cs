using System;
using System.Collections.Generic;

namespace MainGame.PlayersRepos
{
    public interface IPlayersRepo
    {
        event Action<int> PlayersCountChanged;
        
        void Initialize(EventBus eventBus);
        void RegisterPlayer(Player.Player player);
        List<Player.Player> GetPlayersCopy();
    }
}