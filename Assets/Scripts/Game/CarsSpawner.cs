using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

namespace Game
{
    public class CarsSpawner : MonoBehaviour
    {
        [SerializeField] private CarMovement _carPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        private ICarsSpawner _spawner;
        private int _playerIndex = 0;
        private bool _carInitialized;
        
        private void Start()
        {
            int playersCount = StaticParameters.PlayersCount;
            _spawner = GameServices.CreateCarsSpawner();
            _spawner.MustInitialize += InitializeCar;
            
            if(_spawner.CanSpawnCars())
                StartCoroutine(SpawnPlayers(playersCount));

            StartCoroutine(SendReadyMessagesToServer());
        }

        private IEnumerator SendReadyMessagesToServer()
        {
            while (!_carInitialized)
            {
                _spawner.SendReadyMessageToServer();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SpawnPlayers(int count)
        {
            yield return new WaitUntil(() => _spawner.IsAllPlayersReady());
            
            var cameras = new List<Camera>();
            
            for (int i = 0; i < count; i++)
            {
                _playerIndex = i;
                yield return _spawner.Instantiate(_carPrefab, _spawnPoints[i].position, Quaternion.identity, instance =>
                {
                    if (!_spawner.AddCameraAndMovement())
                        return;
                    
                    var camera = instance.GetComponentInChildren<Camera>(true);
                    cameras.Add(camera);
                });
            }
            
            SetCameras(cameras);
        }

        private void InitializeCar(CarMovement movement)
        {
            _carInitialized = true;
            
            if (!_spawner.AddCameraAndMovement())
                return;
                    
            var camera = movement.GetComponentInChildren<Camera>(true);
            if (camera != null)
            {
                camera.gameObject.SetActive(true);
            }
            
            var playerController = movement.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.PlayerIndex = _playerIndex + 1;
                    
                var gameUi = movement.GetComponentInChildren<GameUI>(true);
                playerController.WaitForStart(gameUi);
            }
        }

        private void SetCameras(IList<Camera> cameras)
        {
            switch (cameras.Count)
            {
                case 2:
                    cameras[0].rect = new Rect(0, 0, 0.5f, 1f);
                    cameras[1].rect = new Rect(0.5f, 0f, 0.5f, 1f);
                    break;
                case 3:
                    cameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    cameras[2].rect = new Rect(0.25f, 0f, 0.5f, 0.5f);
                    break;
                case 4:
                    cameras[0].rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                    cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    cameras[2].rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    cameras[3].rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                    break;
            }
        }
    }
}