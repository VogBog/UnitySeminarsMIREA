using System;
using System.Collections;
using Game;
using UnityEngine;

namespace SingleGame
{
    public class TimingKiller : MonoBehaviour
    {
        [field: SerializeField] public int PeaceTime { get; private set; }
        [field: SerializeField] public int TickDuration { get; private set; }
        
        [SerializeField] private TimingKillerView _view;

        private Player _player;

        public event Action<float> StartTickForSeconds;
        public event Action TickConfirmed; 
        
        private void Start()
        {
            _view.Initialize(this);
            StartCoroutine(StartRoutine());

            FindFirstObjectByType<Timer>().Finished += ClearPlayer;
        }

        private IEnumerator StartRoutine()
        {
            yield return new WaitForSeconds(PeaceTime);

            _player = FindFirstObjectByType<Player>();
            _player.Died += _ => ClearPlayer();

            while (_player != null)
            {
                StartTickForSeconds?.Invoke(TickDuration);
                yield return new WaitForSeconds(TickDuration);
                
                if(_player.Movement.IsStopped)
                    _player.GetDamage();
                
                TickConfirmed?.Invoke();
            }
        }

        private void ClearPlayer()
        {
            _player = null;
        }

        private void Update()
        {
            _view.Update(Time.deltaTime);
        }
    }
}