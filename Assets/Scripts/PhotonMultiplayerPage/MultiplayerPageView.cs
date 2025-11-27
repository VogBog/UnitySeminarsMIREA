using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonMultiplayerPage
{
    [Serializable]
    public class MultiplayerPageView
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _createBtn;
        [SerializeField] private Button _joinBtn;
        [SerializeField] private Button _quitBtn;
        
        public event Action<string> CreateClicked;
        public event Action<string> JoinClicked;
        public event Action QuitClicked;
        
        public void Initialize()
        {
            _createBtn.onClick.AddListener(() => CreateClicked?.Invoke(_inputField.text));
            _joinBtn.onClick.AddListener(() => JoinClicked?.Invoke(_inputField.text));
            _quitBtn.onClick.AddListener(() => QuitClicked?.Invoke());
        }

        public void SetActive(bool active)
        {
            _parent.SetActive(active);
        }
    }
}