using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PlayersSpawner : MonoBehaviour
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private CameraFollows _cameraPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        private int _playersCount;

        public event Action End;

        private void Start()
        {
            _playersCount = StaticParameters.PlayersCount;
            bool singlePlayer = StaticParameters.GameType is GameTypes.Single or GameTypes.SplitScreen;
            
            CreatePlayers(singlePlayer ? _playersCount : 1);
        }

        private void CreatePlayers(int count)
        {
            var cameras = new List<Camera>();
            var politics = new PlayersSpawnerPolitics().GetPolitics(this);
            
            for (int i = 0; i < count; i++)
            {
                var instance = politics.Instantiate(
                    _playerPrefab, _spawnPoints[i].position, Quaternion.identity, null);
                instance.Initialize(this);

                instance.Movement.Controller.PlayerIndex = i + 1;
                instance.Died += OnPlayerDie;

                var camera = Instantiate(_cameraPrefab);
                camera.SetTarget(instance);
                
                var rawCamera = camera.GetComponent<Camera>();
                cameras.Add(rawCamera);
                instance.SetCameraToCanvas(rawCamera);
            }
            
            SetCameras(cameras);
        }

        private void SetCameras(IList<Camera> cameras)
        {
            switch (cameras.Count)
            {
                case 2:
                    cameras[0].rect = new Rect(0f, 0f, 0.5f, 1f);
                    cameras[1].rect = new Rect(0.5f, 0f, 0.5f, 1f);
                    break;
                case 3:
                    cameras[0].rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
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

        private void OnPlayerDie(Player player)
        {
            _playersCount--;
            if (_playersCount <= 1)
            {
                End?.Invoke();
                StartCoroutine(EndRoutine());
            }
        }

        private IEnumerator EndRoutine()
        {
            yield return new WaitForSeconds(3f);

            int sceneIndex = 0;
            if (StaticParameters.PlayersCount == 1 && !string.IsNullOrEmpty(StaticParameters.Account.Id))
            {
                sceneIndex = 3;
            }
            
            SceneManager.LoadScene(sceneIndex);
        }
    }
}