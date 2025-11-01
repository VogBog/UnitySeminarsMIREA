using System;
using System.Collections.Generic;
using Extensions;
using MainGame.PlayersRepos;
using UnityEngine;

namespace MainGame
{
    public class PlayersRepo : MonoBehaviour
    {
        private IPlayersRepo _repo;

        public event Action<int> PlayersCountChanged;
        
        public int PlayerIndex => _repo.PlayerIndex;
        public int Count => _repo.Count;

        public PlayersRepo Initialize(IPlayersRepo repo)
        {
            _repo = repo;
            _repo.PlayersCountChanged += i => PlayersCountChanged?.Invoke(i);
            _repo.Initialize(this.FindFirstObjectByTypeOrException<EventBus>());
            
            return this;
        }

        public void RegisterPlayer(Player.Player player, bool isMy) => _repo.RegisterPlayer(player, isMy);

        public List<Player.Player> GetPlayersCopy() => _repo.GetPlayersCopy();

        public Player.Player Get(int index) => _repo.Get(index);
        public int Get(Player.Player player) => _repo.Get(player);
    }
}