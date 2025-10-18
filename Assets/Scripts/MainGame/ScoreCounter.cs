using System.Collections.Generic;
using System.Linq;
using Damage;
using Data;
using Extensions;
using FirebaseDesktopHelper;
using Player;
using UnityEngine;

namespace MainGame
{
    public class ScoreCounter
    {
        private readonly Player.Player _player;
        private readonly List<(string, float)> _scoreAdds = new();
        private readonly Dictionary<int, int> _damageScore = new();

        private readonly GameTimer _timer;
        
        public ScoreCounter(Player.Player player)
        {
            _player = player;
            _timer = player.FindFirstObjectByTypeOrException<GameTimer>();
            
            var eventBus = player.FindFirstObjectByTypeOrException<EventBus>();
            var gameFinisher = player.FindFirstObjectByTypeOrException<GameFinisher>();

            gameFinisher.Finished += OnGameFinished;
            gameFinisher.PlayerWinned += OnPlayerWinned;
            eventBus.PlayerTakeDamage += OnPlayerTakeDamage;
            eventBus.PlayerKilledByAnotherPlayer += OnPlayerKilledAnother;
        }

        public void OnGameFinished()
        {
            if (_timer.Seconds >= 180)
                return;
            float addScore = (180 - _timer.Seconds) / 18f;
            _scoreAdds.Add(($"Quick round {_timer.Seconds}s", addScore));

            if(!StaticParameters.SinglePlayer)
                SaveScore();
        }

        public void OnPlayerWinned(Player.Player player)
        {
            if(_player == player)
                _scoreAdds.Add(("Win", 30));
        }

        public void OnPlayerTakeDamage(Player.Player player, ref GetDamageData data)
        {
            if (player == _player)
            {
                string option = "Take damage";
                int index = _scoreAdds.FindIndex(x => x.Item1.StartsWith(option));
                if (index == -1)
                {
                    _scoreAdds.Add((option, -1.5f));
                }
                else
                {
                    float score = _scoreAdds[index].Item2;
                    score -= 1.5f;
                    int count = Mathf.RoundToInt(-score / 1.5f);
                    _scoreAdds[index] = ($"{option} x{count}", score);
                }

                return;
            }
            
            if (data.Attacker != _player.gameObject)
                return;

            _damageScore.TryAdd(data.Damage, 0);
            _damageScore[data.Damage]++;
        }

        public void OnPlayerKilledAnother(PlayerHealth health, Player.Player killer)
        {
            if (killer != _player)
                return;

            string option = "Kill";
            int index = _scoreAdds.FindIndex(x => x.Item1.StartsWith(option));
            if (index == -1)
            {
                _scoreAdds.Add((option, 3));
            }
            else
            {
                float score = _scoreAdds[index].Item2;
                score += 3;
                int count = Mathf.RoundToInt(score / 3);
                _scoreAdds[index] = ($"{option} x{count}", score);
            }
        }

        public List<(string, float)> CalculateScore(out float totalScore)
        {
            var result = new List<(string, float)>(_scoreAdds);
            foreach (var kvp in _damageScore)
            {
                float addScore = kvp.Key * kvp.Value;
                if (kvp.Key == 2) addScore *= 1.2f;
                if (kvp.Key == 3) addScore *= 1.3f;
                if (kvp.Key == 4) addScore *= 1.4f;
                if (kvp.Key >= 5) addScore *= 1.6f;
                
                result.Add(($"Damaged {kvp.Key} x{kvp.Value}", addScore));
            }

            totalScore = result.Sum(x => x.Item2);
            return result;
        }

        public void SaveScore()
        {
            if (StaticParameters.SinglePlayer || StaticParameters.GameType is GameType.None)
                return;
            
            var playerAccount = StaticParameters.PlayerAccount;
            StaticParameters.PlayerScore = CalculateScore(out float score);
            
            if(StaticParameters.GameType is GameType.P1Vs1 && playerAccount.Max1V1Score < score)
                playerAccount.Max1V1Score = score;
            else if(StaticParameters.GameType is GameType.P1Vs3 && playerAccount.Max4Score < score)
                playerAccount.Max4Score = score;
            else if (StaticParameters.GameType is GameType.P2Vs2 && playerAccount.Max2V2Score < score)
                playerAccount.Max2V2Score = score;
            else
                return;
            
            StaticParameters.PlayerAccount = playerAccount;
            FirebaseRestRequests.RealtimeDatabase.Query()
                .GetAccounts()
                .GetChild(playerAccount.Id)
                .SetJson(playerAccount)
                .Call()
                .Put()
                .Void()
                .Empty();
        }
    }
}