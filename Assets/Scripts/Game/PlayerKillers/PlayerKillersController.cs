using System.Collections.Generic;
using System.Linq;
using Base;
using UnityEngine;

namespace Game.PlayerKillers
{
    public class PlayerKillersController : MonoBehaviour
    {
        private IPlayerKiller _killer;
        private List<Player.Player> _players;
        private List<Player.Player> _buffer = new();

        private void Awake()
        {
            _killer = GameServices.CreatePlayerKiller();
            FindFirstObjectByType<GameStarter.GameStarter>().Started += OnStart;
        }

        private void OnStart()
        {
            _players = FindObjectsByType<Player.Player>(FindObjectsSortMode.None).ToList();
        }

        private void FixedUpdate()
        {
            if (_players == null || !_killer.IsServer())
                return;
            
            _buffer.Clear();

            for (int i = 0; i < _players.Count; i++)
            {
                for (int j = 0; j < _players.Count; j++)
                {
                    var player1 = _players[i];
                    var player2 = _players[j];
                    
                    if((player2.SnakeTail.IsInDanger(player1.transform.position, player1 != player2) ||
                        IsOnDangerZone(player1.transform.position)) &&
                       !_buffer.Contains(player1))
                        _buffer.Add(player1);
                }
            }

            foreach (var player in _buffer)
            {
                player.Die();
                _players.Remove(player);
            }

            if (_players.Count == 0 && _buffer.Count > 0)
            {
                FindFirstObjectByType<GameFinisher.GameFinisher>()?.AllPlayersDied();
            }
        }

        private bool IsOnDangerZone(Vector3 pos)
        => pos.x <= -49 || pos.x >= 49 || pos.z <= -49 || pos.z >= 49;
    }
}