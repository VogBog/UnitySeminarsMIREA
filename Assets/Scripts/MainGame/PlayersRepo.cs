using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace MainGame
{
    public class PlayersRepo : MonoBehaviour
    {
        private readonly List<Player.Player> _players = new();

        public event Action<int> PlayersCountChanged; 
        
        public void RegisterPlayer(Player.Player player)
        {
            _players.Add(player);
            player.Health.Died += OnPlayerDie;
        }

        private void OnPlayerDie(PlayerHealth playerHealth)
        {
            var player = _players.Find(x => x.Health == playerHealth);
            if (player == null)
            {
                Debug.LogError($"PlayersRepo: Cannot find died player: {playerHealth}");
                return;
            }
            
            _players.Remove(player);

            PlayersCountChanged?.Invoke(_players.Count);
        }

        public List<Player.Player> GetPlayersCopy() => new(_players);
    }
}