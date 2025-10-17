using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Account
{
    [Serializable]
    public class AccountWelcomeScreen
    {
        [Header("Screens")]
        [SerializeField] private GameObject _welcomeScreen;
        [SerializeField] private GameObject _authScreen;
        
        [Space] [Header("Welcome Screen")]
        [SerializeField] private Button _logInButton;
        [SerializeField] private Button _signUpButton;
        [SerializeField] private Button _singlePlayerButton;

        [Space] [Header("Auth Screen")]
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _cancelButton;

        private bool _isSigningUp;

        public event Action<string, string, string> SignUpNicknameEmailPassword;
        public event Action<string, string> LogInEmailPassword;
        public event Action SinglePlayerChoose;
        
        public const string SignUpText = "Sign Up";
        public const string LogInText = "Log In";
        
        public void Initialize()
        {
            _logInButton.onClick.AddListener(OnLogInClicked);
            _signUpButton.onClick.AddListener(OnSignUpClicked);
            _singlePlayerButton.onClick.AddListener(OnSinglePlayerClicked);
            
            _submitButton.onClick.AddListener(OnSubmitClicked);
            _cancelButton.onClick.AddListener(OnCancelClicked);
            
            _welcomeScreen.SetActive(false);
            _authScreen.SetActive(false);
            
            _errorText.gameObject.SetActive(false);
        }

        public void ShowWelcomeScreen()
        {
            _welcomeScreen.SetActive(true);
            _authScreen.SetActive(false);
        }

        public void ShowAuthScreen()
        {
            if(_isSigningUp)
                OnSignUpClicked();
            else
                OnLogInClicked();
        }

        public void ShowErrorText(string message)
        {
            _errorText.text = message;
            _errorText.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            _welcomeScreen.SetActive(false);
            _authScreen.SetActive(false);
        }

        private void OnLogInClicked()
        {
            _isSigningUp = false;
            OpenAuthScreen(false, LogInText);
        }

        private void OnSignUpClicked()
        {
            _isSigningUp = true;
            OpenAuthScreen(true, SignUpText);
        }

        private void OpenAuthScreen(bool isUsernameActive, string mainText)
        {
            _welcomeScreen.SetActive(false);
            _authScreen.SetActive(true);
            _usernameInputField.gameObject.SetActive(isUsernameActive);
            _mainText.text = mainText;
        }

        private void OnSinglePlayerClicked()
        {
            _welcomeScreen.SetActive(false);
            _authScreen.SetActive(false);
            
            SinglePlayerChoose?.Invoke();
        }

        private void OnSubmitClicked()
        {
            if (string.IsNullOrEmpty(_emailInputField.text) ||
                string.IsNullOrWhiteSpace(_emailInputField.text) ||
                string.IsNullOrEmpty(_passwordInputField.text) ||
                string.IsNullOrWhiteSpace(_passwordInputField.text))
                return;

            if (_isSigningUp &&
                !string.IsNullOrEmpty(_usernameInputField.text) &&
                !string.IsNullOrWhiteSpace(_usernameInputField.text))
            {
                HideAll();
                SignUpNicknameEmailPassword?.Invoke(
                    _usernameInputField.text,
                    _emailInputField.text,
                    _passwordInputField.text);
                return;
            }

            if (!_isSigningUp)
            {
                HideAll();
                LogInEmailPassword?.Invoke(
                    _emailInputField.text,
                    _passwordInputField.text);
            }
        }

        private void OnCancelClicked()
        {
            ShowWelcomeScreen();
        }
    }
}