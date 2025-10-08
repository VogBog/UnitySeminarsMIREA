using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    [Serializable]
    public class PlayerNameInput
    {
        [SerializeField] private GameObject _screen;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _applyBtn;

        public event Action<string> Submitted; 

        public void Initialize()
        {
            _applyBtn?.onClick.AddListener(OnApplyBtnClicked);
            _inputField?.onSubmit.AddListener(_ => OnApplyBtnClicked());
            _inputField?.onValueChanged.AddListener(OnValueChanged);
        }
        
        public void ShowInputField(int playerIndex)
        {
            _screen.SetActive(true);
            _text.text = $"Input player {playerIndex} name";
            _inputField.text = "";
            _applyBtn.interactable = false;
        }

        private void OnValueChanged(string value)
        {
            _applyBtn.interactable = !string.IsNullOrEmpty(value);
        }

        private void OnApplyBtnClicked()
        {
            if (string.IsNullOrEmpty(_inputField.text))
                return;
            
            _screen.SetActive(false);
            Submitted?.Invoke(_inputField.text);
        }
    }
}