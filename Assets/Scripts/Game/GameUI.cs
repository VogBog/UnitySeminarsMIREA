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

        public event Action RaceStarted;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            var controls = PlayerController.GetPlayerCodes(_playerController.PlayerIndex);
            string text = $"{controls.Item1}{controls.Item2}{controls.Item3}{controls.Item4}";

            if (controls.Item1 == KeyCode.UpArrow)
                text = $"Arrows";
            
            _text.text = text;

            yield return new WaitForSeconds(2f);

            for (int timer = 3; timer > 0; timer--)
            {
                _text.text = timer.ToString();
                
                yield return new WaitForSeconds(1f);
            }

            _text.text = "GO";
            RaceStarted?.Invoke();
            
            yield return new WaitForSeconds(1f);
            
            _text.gameObject.SetActive(false);

            var checkPoints = FindFirstObjectByType<CheckPointsHolder>();
            checkPoints.CarFinished += OnFinish;
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