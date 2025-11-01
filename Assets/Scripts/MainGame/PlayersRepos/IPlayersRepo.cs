using System;
using System.Collections.Generic;

namespace MainGame.PlayersRepos
{
    public interface IPlayersRepo
    {
        event Action<int> PlayersCountChanged;
        
        int Count { get; }
        int PlayerIndex { get; }
        
        void Initialize(EventBus eventBus);
        void RegisterPlayer(Player.Player player, bool isMy);
        List<Player.Player> GetPlayersCopy();
        Player.Player Get(int index);
        int Get(Player.Player player);
    }
}