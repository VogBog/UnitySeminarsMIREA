using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class CheckPointsHolder : MonoBehaviour
    {
        [SerializeField] private CheckPoint[] _orderedCheckPoints;
        [SerializeField] private int _roundsCount = 3;

        private List<CheckPoint> _checkPoints;
        private IGameFinisher _gameFinisher;
        private int _finishedCars;

        public event Action<CarMapRunner, int> CarFinished; 
        
        private void Awake()
        {
            _gameFinisher = GameServices.CreateGameFinisher();
            
            foreach (var checkPoint in _orderedCheckPoints)
            {
                checkPoint.Collided += OnCheckPointCollided;
            }

            _checkPoints = _orderedCheckPoints.ToList();
        }

        private void OnCheckPointCollided(CarMapRunner car, CheckPoint checkPoint)
        {
            int index = _checkPoints.IndexOf(checkPoint);

            if (index == -1)
                return;

            if (index > car.CheckPointIndex ||
                (index == 0 && car.CheckPointIndex == _checkPoints.Count - 1))
            {
                car.GoThroughCheckPoint(index);
            }

            if (car.TryFinish(_roundsCount))
            {
                _finishedCars++;
                CarFinished?.Invoke(car, _finishedCars);

                if (_finishedCars == StaticParameters.PlayersCount)
                {
                    StartCoroutine(FinishRoutine());
                }
            }
        }

        private IEnumerator FinishRoutine()
        {
            StaticParameters.FinishData = CreateFinishData();
            yield return new WaitForSeconds(3f);
            
            if(!_gameFinisher.CanFinish())
                yield break;
            
            _gameFinisher.OnBeforeLoadingScene();
            SceneManager.LoadScene(2);
        }

        private StaticParameters.PlayerFinishData[] CreateFinishData()
        {
            var cars = _gameFinisher.GetRunnersForRecord();
            var result = new StaticParameters.PlayerFinishData[cars.Length];

            for (int i = 0; i < result.Length; i++)
            {
                var playerController = cars[i].GetComponentInChildren<PlayerController>();
                var timer = cars[i].GetComponentInChildren<GameTimer>();

                result[i] = new(playerController.PlayerIndex, timer.Time);
            }

            return result;
        }
    }
}