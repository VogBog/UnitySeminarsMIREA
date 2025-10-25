using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Extensions;
using MainGame.GameFinishers;
using UnityEngine;

namespace MainGame
{
    public class GameFinisher : MonoBehaviour
    {
        private bool _ended = false;
        private IGameFinisher _gameFinisher;
        private PlayersRepo _playersRepo;
        private GameTimer _gameTimer;

        public event Action<Player.Player> PlayerWinned; 
        public event Action Finished;
        
        public GameFinisher Initialize(IGameFinisher finisher)
        {
            _playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
            _gameTimer = this.FindFirstObjectByTypeOrException<GameTimer>();
            _playersRepo.PlayersCountChanged += OnPlayersCountChanged;
            
            _gameFinisher = finisher;
            _gameFinisher.Finished += () => Finished?.Invoke();
            _gameFinisher.PlayerWinned += p => PlayerWinned?.Invoke(p);

            return this;
        }

        private void OnPlayersCountChanged(int count)
        {
            if(count <= 1)
                EndGame(_playersRepo.GetPlayersCopy());
        }

        public void EndGame(IEnumerable<Player.Player> winners)
        {
            if (_ended || !_gameFinisher.CanFinishGame())
                return;
            _ended = true;

            _gameTimer.StopTimer();
            StartCoroutine(EndGameRoutine());
            
            foreach(var winner in winners)
                _gameFinisher.InvokePlayerWinned(winner);
            
            _gameFinisher.InvokeFinished();
        }

        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSeconds(4f);

            if (StaticParameters.PlayerScore == null)
            {
                _gameFinisher.LoadScene(1);
            }
            else
            {
                _gameFinisher.LoadScene(3);
            }
        }
    }
}