using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SingleGame
{
    [Serializable]
    public class ScoreCounterView
    {
        [SerializeField] private TMP_Text _added;
        [SerializeField] private TMP_Text _total;

        private ScoreCounter _counter;
        private Coroutine _coroutine;
        
        public void Initialize(ScoreCounter counter)
        {
            _counter = counter;
            counter.AddedAndTotal += OnAdded;

            _added.text = "";
        }

        private void OnAdded(float added, float total)
        {
            _added.text = "+" + added.ToString("N0");
            _total.text = total.ToString("N0");
            
            if(_coroutine != null)
                _counter.StopCoroutine(_coroutine);
            _coroutine = _counter.StartCoroutine(AnimationRoutine());
        }

        private IEnumerator AnimationRoutine()
        {
            yield return new WaitForSeconds(3f);
            _added.text = "";
        }
    }
}