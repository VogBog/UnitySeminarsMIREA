using System;
using UnityEngine;

namespace Game
{
    public class GameTimer : MonoBehaviour
    {
        private GameUI _ui;
        private CheckPointsHolder _checkPoints;
        
        public float Time { get; private set; }
        public bool Active { get; private set; }

        public event Action<float> Stopped;

        private void Awake()
        {
            var gameUI = GetComponentInChildren<GameUI>();
            var checkPoints = FindFirstObjectByType<CheckPointsHolder>();
            var car = GetComponentInChildren<CarMapRunner>();
            
            Initialize(gameUI, checkPoints, car);
        }

        public void Initialize(GameUI gameUI, CheckPointsHolder checkPoints, CarMapRunner car)
        {
            _ui = gameUI;
            _checkPoints = checkPoints;

            _ui.RaceStarted += StartTimer;
            _checkPoints.CarFinished += (runner, _) =>
            {
                if (car == runner)
                    StopTimer();
            };
        }

        public void StartTimer()
        {
            Active = true;
        }

        public void StopTimer()
        {
            Active = false;
            Stopped?.Invoke(Time);
        }

        private void Update()
        {
            if (!Active)
                return;

            Time += UnityEngine.Time.deltaTime;
        }
    }
}