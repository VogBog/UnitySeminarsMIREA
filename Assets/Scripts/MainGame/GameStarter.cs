using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Extensions;
using Lobby;
using MainGame.GameStarters;
using MainGame.Initializers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private Transform _spawnPointsParent;
        [SerializeField] private float _addYForEveryPlayer;

        private IGameStarter _gameStarter;
        private PlayersRepo _playersRepo;
        private GameTimer _gameTimer;
        
        public Player.Player PlayerPrefab => _playerPrefab;

        private void Start()
        {
            _playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
            _gameTimer = this.FindFirstObjectByTypeOrException<GameTimer>();

            _gameStarter = ServicesInitializer.InitializeServices();
            
            var players = StaticParameters.Players;
            
            if (players == null || players.Length == 0)
            {
                Debug.LogError("Something went wrong. Cannot get players array. Try open this scene from Main Menu.");
                return;
            }
            
            StartCoroutine(InitializePlayers(players));
        }

        private List<Vector3> GetAllSpawnPoints()
        {
            var result = new List<Vector3>(_spawnPointsParent.childCount);
            for (int i = 0; i < _spawnPointsParent.childCount; i++)
            {
                var child = _spawnPointsParent.GetChild(i);
                result.Add(child.position);
            }

            return result;
        }

        private IEnumerator InitializePlayers(PlayerData[] players)
        {
            var spawnPoints = GetAllSpawnPoints();
            var instances = new List<Player.Player>();

            foreach (var data in players)
            {
                if(!data.IsActive)
                    continue;
                
                int randIndex = Random.Range(0, spawnPoints.Count);
                var point = spawnPoints[randIndex];
                spawnPoints.RemoveAt(randIndex);

                var dataForLambda = data;
                
                yield return _gameStarter.CreatePlayerRoutine(this, point, instance =>
                {
                    instances.Add(instance);
                    _playersRepo.RegisterPlayer(instance, true);
                    StartCoroutine(InitializePlayerDelayed(instance, dataForLambda));
                });
            }
            
            SetCameras(instances);
            
            _gameTimer.StartTimer();
        }

        private IEnumerator InitializePlayerDelayed(Player.Player player, PlayerData data)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            
            player.Initialize(data);
        }

        private void SetCameras(IList<Player.Player> players)
        {
            switch (players.Count)
            {
                case 1:
                    players[0].Camera.rect = new Rect(0, 0, 1, 1);
                    break;
                case 2:
                    players[0].Camera.rect = new Rect(0, 0, .5f, 1);
                    players[1].Camera.rect = new Rect(.5f, 0, .5f, 1);
                    break;
                case 3:
                    players[0].Camera.rect = new Rect(0f, 0f, .5f, .5f);
                    players[1].Camera.rect = new Rect(0.5f, 0f, .5f, .5f);
                    players[2].Camera.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);
                    break;
                case 4:
                    players[0].Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    players[1].Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    players[2].Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    players[3].Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                default:
                    throw new Exception("Max count of players is 4");
            }

            if (players.Count == 1)
                return;

            foreach (var player in players)
            {
                player.Camera.transform.localPosition -=
                    player.Camera.transform.forward * _addYForEveryPlayer * (players.Count - 1);
            }
        }
    }
}