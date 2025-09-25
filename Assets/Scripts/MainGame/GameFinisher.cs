using System.Collections;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    public class GameFinisher : MonoBehaviour
    {
        private bool _ended = false;
        
        private void Awake()
        {
            var playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
            playersRepo.PlayersCountChanged += OnPlayersCountChanged;
        }

        private void OnPlayersCountChanged(int count)
        {
            if(count <= 1)
                EndGame();
        }

        public void EndGame()
        {
            if (_ended)
                return;
            _ended = true;

            StartCoroutine(EndGameRoutine());
        }

        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(0);
        }
    }
}