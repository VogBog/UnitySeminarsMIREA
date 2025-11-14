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
        [SerializeField] private Transform _spawnPoints;
        [SerializeField] private TMP_Text _timerText;

        private IGameStarter _starter;
        private bool _started = false;

        public event Action Started;

        private void Start()
        {
            _starter = GameServices.CreateGameStarter();
            _starter.TimerChanged += OnTimerChanged;
            _starter.Initialized += InitializePlayer;

            StartCoroutine(InvokeConnectedRoutine());

            if (_starter.IsServer())
                StartCoroutine(SpawnPlayers(StaticParameters.PlayersCount));
        }

        private IEnumerator InvokeConnectedRoutine()
        {
            while (!_started)
            {
                _starter.InvokePlayerConnected();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator SpawnPlayers(int playersCount)
        {
            yield return new WaitUntil(() => _starter.IsAllPlayersConnected());
            
            var spawnPoints = _spawnPoints.GetComponentsInChildren<Transform>()
                .Select(x => x.position).ToList();
            
            for (int i = 0; i < playersCount; i++)
            {
                int randIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
                var spawnPoint = spawnPoints[randIndex];
                spawnPoints.RemoveAt(randIndex);

                yield return _starter.InstantiatePlayer(
                    _playerPrefab,
                    spawnPoint,
                    Quaternion.identity);
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
                _started = true;
                Started?.Invoke();
            }
        }

        private void InitializePlayer(Player.Player player)
        {
            var camera = player.GetComponentInChildren<Camera>(true);
            camera.gameObject.SetActive(true);
        }
    }
}