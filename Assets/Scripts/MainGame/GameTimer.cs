using System.Collections;
using UnityEngine;

namespace MainGame
{
    public class GameTimer : MonoBehaviour
    {
        public int Seconds { get; private set; }
        
        private Coroutine _coroutine;

        public void StartTimer()
        {
            if(_coroutine != null)
                return;
            
            _coroutine = StartCoroutine(TimerRoutine());
        }

        public void StopTimer()
        {
            if (_coroutine == null)
                return;
            
            StopCoroutine(_coroutine);
        }
        
        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                
                Seconds++;
            }
        }
    }
}