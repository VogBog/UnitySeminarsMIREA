using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    public class GameFinisher : MonoBehaviour
    {
        private bool _ended = false;
        private PlayersRepo _playersRepo;
        private GameTimer _gameTimer;

        public event Action<Player.Player> PlayerWinned; 
        public event Action Finished;
        
        private void Awake()
        {
            _playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
            _gameTimer = this.FindFirstObjectByTypeOrException<GameTimer>();
            _playersRepo.PlayersCountChanged += OnPlayersCountChanged;
        }

        private void OnPlayersCountChanged(int count)
        {
            if(count <= 1)
                EndGame(_playersRepo.GetPlayersCopy());
        }

        public void EndGame(IEnumerable<Player.Player> winners)
        {
            if (_ended)
                return;
            _ended = true;

            _gameTimer.StopTimer();
            StartCoroutine(EndGameRoutine());
            
            foreach(var winner in winners)
                PlayerWinned?.Invoke(winner);
            
            Finished?.Invoke();
        }

        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSeconds(4f);

            if (StaticParameters.PlayerScore == null)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(3);
            }
        }
    }
}