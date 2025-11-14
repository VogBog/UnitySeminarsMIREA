using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CarMovement _carMovement;
        [SerializeField] private CarMapRunner _carRunner;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _speedText;

        private bool _stopTimer = false;

        public event Action<int> TimerChanged; 
        public event Action RaceStarted;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            var controls = PlayerController.GetPlayerCodes(_playerController.PlayerIndex);
            string text = $"{controls.Item1}{controls.Item2}{controls.Item3}{controls.Item4}";

            if (controls.Item1 == KeyCode.UpArrow)
                text = $"Arrows";
            
            _text.text = text;

            yield return new WaitForSeconds(1f);

            if (_stopTimer) yield break;

            yield return new WaitForSeconds(1f);

            if (_stopTimer) yield break;

            for (int timer = 3; timer > 0; timer--)
            {
                _text.text = timer.ToString();
                TimerChanged?.Invoke(timer);
                
                yield return new WaitForSeconds(1f);

                if (_stopTimer) yield break;
            }

            TimerChanged?.Invoke(0);
            StopTimerAndUpdate(0);
        }

        private IEnumerator AfterStartRoutine()
        {
            yield return new WaitForSeconds(1f);
            
            _text.gameObject.SetActive(false);
            var checkPoints = FindFirstObjectByType<CheckPointsHolder>();
            checkPoints.CarFinished += OnFinish;
        }

        public void StopTimerAndUpdate(int time)
        {
            _text.text = time.ToString();
            if (time == 0)
            {
                _text.text = "GO";
                RaceStarted?.Invoke();
                StartCoroutine(AfterStartRoutine());
            }
        }

        private void Update()
        {
            _speedText.text = ((int)_carMovement.Velocity).ToString();
        }

        private void OnFinish(CarMapRunner car, int finish)
        {
            if (car != _carRunner)
                return;
            
            _text.text = $"Finish: {finish}";
            _text.gameObject.SetActive(true);
        }
    }
}