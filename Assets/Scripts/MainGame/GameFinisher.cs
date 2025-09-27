using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    public class GameFinisher : MonoBehaviour
    {
        private bool _ended = false;
        private PlayersRepo _playersRepo;

        public event Action<Player.Player> PlayerWinned; 
        
        private void Awake()
        {
            _playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
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

            StartCoroutine(EndGameRoutine());
            
            foreach(var winner in winners)
                PlayerWinned?.Invoke(winner);
        }

        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSeconds(4f);

            SceneManager.LoadScene(0);
        }
    }
}