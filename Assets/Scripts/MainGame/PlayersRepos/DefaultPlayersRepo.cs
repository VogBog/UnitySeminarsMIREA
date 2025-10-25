using System;
using System.Collections.Generic;

namespace MainGame.PlayersRepos
{
    public class DefaultPlayersRepo : IPlayersRepo
    {
        private readonly List<Player.Player> _players = new();

        public event Action<int> PlayersCountChanged;

        public void Initialize(EventBus eventBus)
        {
            eventBus.PlayerDied += OnPlayerDie;
        }
        
        public void RegisterPlayer(Player.Player player)
        {
            _players.Add(player);
        }

        private void OnPlayerDie(Player.Player player)
        {
            _players.Remove(player);

            PlayersCountChanged?.Invoke(_players.Count);
        }

        public List<Player.Player> GetPlayersCopy() => new(_players);
    }
}