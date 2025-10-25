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

        public PlayersRepo Initialize(IPlayersRepo repo)
        {
            _repo = repo;
            _repo.PlayersCountChanged += i => PlayersCountChanged?.Invoke(i);
            _repo.Initialize(this.FindFirstObjectByTypeOrException<EventBus>());
            
            return this;
        }
        
        public void RegisterPlayer(Player.Player player) => _repo.RegisterPlayer(player);

        public List<Player.Player> GetPlayersCopy() => _repo.GetPlayersCopy();
    }
}