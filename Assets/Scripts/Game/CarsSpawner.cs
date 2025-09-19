using System.Collections.Generic;
using Global;
using UnityEngine;

namespace Game
{
    public class CarsSpawner : MonoBehaviour
    {
        [SerializeField] private CarMovement _carPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        
        private void Start()
        {
            int playersCount = StaticParameters.PlayersCount;
            SpawnPlayers(playersCount);
        }

        private void SpawnPlayers(int count)
        {
            var cameras = new List<Camera>();
            
            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(_carPrefab, _spawnPoints[i].position, Quaternion.identity);

                var camera = instance.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    cameras.Add(camera);
                }
                
                var playerController = instance.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.PlayerIndex = i + 1;
                    
                    var gameUi = instance.GetComponentInChildren<GameUI>();
                    playerController.WaitForStart(gameUi);
                }
            }
            
            SetCameras(cameras);
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