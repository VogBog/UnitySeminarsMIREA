using System;
using System.Collections;
using UnityEngine;

namespace MainGame.GameTimers
{
    public class DefaultGameTimer : IGameTimer
    {
        public int Seconds { get; private set; }
        
        private Coroutine _coroutine;
        
        public event Action<int> SecondsChanged; 

        public void StartTimer(MonoBehaviour monoBeh)
        {
            if(_coroutine != null)
                return;
            
            _coroutine = monoBeh.StartCoroutine(TimerRoutine());
        }

        public void StopTimer(MonoBehaviour monoBeh)
        {
            if (_coroutine == null)
                return;
            
            monoBeh.StopCoroutine(_coroutine);
        }
        
        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                
                Seconds++;
                SecondsChanged?.Invoke(Seconds);
            }
        }
    }
}