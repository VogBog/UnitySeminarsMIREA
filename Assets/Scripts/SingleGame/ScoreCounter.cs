using System;
using System.Collections;
using Game;
using UnityEngine;

namespace SingleGame
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private ScoreCounterView _view;
        private Player _player;

        public float Score { get; private set; }

        public event Action<float, float> AddedAndTotal; 

        private IEnumerator Start()
        {
            _view.Initialize(this);
            
            yield return null;

            _player = FindFirstObjectByType<Player>();
            
            var killPoints = FindObjectsByType<KillPoint>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var killPoint in killPoints)
            {
                killPoint.InteractedWithSpeed += OnKillPointInteractedWithSpeed;
            }
        }

        private void OnKillPointInteractedWithSpeed(KillPoint killPoint, float speed)
        {
            float addSpeed = speed - _player.Movement.Speed;
            if (addSpeed > 0)
            {
                _player.Movement.AddSpeed(addSpeed);
            }

            Score += speed;
            AddedAndTotal?.Invoke(speed, Score);
        }
    }
}