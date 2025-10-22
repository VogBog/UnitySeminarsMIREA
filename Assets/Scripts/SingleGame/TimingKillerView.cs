using System;
using UnityEngine;
using UnityEngine.UI;

namespace SingleGame
{
    [Serializable]
    public class TimingKillerView
    {
        [SerializeField] private Image[] _bars;

        private float _timer;
        private float _maxTimer;
        private float _peaceTime;
        private bool _isPeaceTime;

        public void Initialize(TimingKiller killer)
        {
            killer.StartTickForSeconds += StartAnimation;
            killer.TickConfirmed += TickConfirmed;
            
            _peaceTime = killer.PeaceTime;
            _maxTimer = killer.TickDuration;
            _timer = killer.PeaceTime;
            _isPeaceTime = true;
        }

        private void StartAnimation(float time)
        {
            _timer = time;
            _isPeaceTime = false;
        }

        private void TickConfirmed()
        {
            foreach (var bar in _bars)
            {
                bar.fillAmount = 1f;
            }
        }

        public void Update(float deltaTime)
        {
            if (_timer <= 0)
                return;

            float max = _isPeaceTime ? _peaceTime : _maxTimer;
            _timer = Mathf.Clamp(_timer - deltaTime, 0f, max);
            
            foreach (var bar in _bars)
            {
                bar.fillAmount = 1f - (_timer / max);
            }
        }
    }
}