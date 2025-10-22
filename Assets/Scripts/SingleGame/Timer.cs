using System;
using System.Collections;
using Game;
using UnityEngine;

namespace SingleGame
{
    public class Timer : MonoBehaviour
    {
        [field: SerializeField] public int MaxTime { get; private set; }

        [SerializeField] private TimerView _view;
        
        public int CurTime { get; private set; }

        public event Action<int> Changed;
        public event Action Finished;
        
        private void Start()
        {
            _view.Initialize(this);
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            CurTime = MaxTime;
            Changed?.Invoke(CurTime);
            
            for (int i = MaxTime; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);

                CurTime = Mathf.Clamp(CurTime - 1, 0, MaxTime);
                Changed?.Invoke(CurTime);
            }
            
            Finished?.Invoke();

            var player = FindFirstObjectByType<Player>();

            while (player.Health > 0)
            {
                player.GetDamage();

                yield return new WaitForSeconds(1f);
            } 
        }
    }
}