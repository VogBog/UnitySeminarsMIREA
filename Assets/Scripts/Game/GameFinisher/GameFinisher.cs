using System.Collections;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameFinisher
{
    public class GameFinisher : MonoBehaviour
    {
        private IGameFinisher _finisher;
        
        private void Awake()
        {
            _finisher = GameServices.CreateGameFinisher();
            _finisher.Died += OnDied;
            
            FindFirstObjectByType<GameStarter.GameStarter>().Started += () =>
            {
                var players = FindObjectsByType<Player.Player>(FindObjectsSortMode.None);
                
                foreach (var player in players)
                {
                    player.Died += OnPlayerDied;
                }
            };
        }

        private void OnPlayerDied(Player.Player player)
        {
            _finisher.InvokeDiedToOwner(player);
        }

        private void OnDied()
        {
            StartCoroutine(DiedRoutine(false));
        }

        private IEnumerator DiedRoutine(bool allPlayersDied)
        {
            yield return new WaitForSeconds(3f);

            if (_finisher.IsServer() && !allPlayersDied)
                yield break;
            
            if(_finisher.IsServer())
                _finisher.ServerQuitFromGame();
            else
                _finisher.QuitFromGame();
            
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }

        public void AllPlayersDied()
        {
            if (!_finisher.IsServer())
                return;

            StartCoroutine(DiedRoutine(true));
        }
    }
}