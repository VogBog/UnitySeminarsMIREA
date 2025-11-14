using System;
using System.Collections;
using System.Linq;
using Base;
using TMPro;
using UnityEngine;

namespace Game.GameStarter
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private TMP_Text _timerText;

        private IGameStarter _starter;

        public event Action Started;

        private void Start()
        {
            _starter = GameServices.CreateGameStarter();
            _starter.TimerChanged += OnTimerChanged;

            if (_starter.IsServer())
                StartCoroutine(SpawnPlayers(StaticParameters.PlayersCount));
        }

        private IEnumerator SpawnPlayers(int playersCount)
        {
            var spawnPoints = _spawnPoints.Select(x => x.position).ToList();
            
            for (int i = 0; i < playersCount; i++)
            {
                int randIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
                var spawnPoint = spawnPoints[randIndex];
                spawnPoints.RemoveAt(randIndex);

                yield return _starter.InstantiatePlayer(
                    _playerPrefab,
                    spawnPoint,
                    Quaternion.identity,
                    player =>
                    {

                    });
            }
            
            _timerText.gameObject.SetActive(true);
            for (int timer = 3; timer > 0; timer--)
            {
                _timerText.text = timer.ToString();
                _starter.InvokeTimerChanged(timer);
                yield return new WaitForSeconds(1f);
            }
            
            _starter.InvokeTimerChanged(0);
        }

        private void OnTimerChanged(int value)
        {
            _timerText.gameObject.SetActive(true);
            _timerText.text = value.ToString();
            if (value == 0)
            {
                _timerText.gameObject.SetActive(false);
                Started?.Invoke();
            }
        }
    }
}