using System;
using TMPro;
using UnityEngine;

namespace SingleGame
{
    [Serializable]
    public class TimerView
    {
        [SerializeField] private TMP_Text _timerText;

        public void Initialize(Timer timer)
        {
            timer.Changed += SetTimer;
            timer.Finished += FinishTimer;
        }

        private void SetTimer(int number)
        {
            _timerText.text = number.ToString();
        }

        private void FinishTimer()
        {
            _timerText.text = "THE END";
        }
    }
}