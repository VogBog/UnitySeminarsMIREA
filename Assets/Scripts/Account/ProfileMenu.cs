using System;
using Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Account
{
    [Serializable]
    public class ProfileMenu
    {
        [Header("Main")]
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _menu;
        [SerializeField] private Button _menuBtn;
        [SerializeField] private Button _registerBtn;
        [SerializeField] private Button _loginBtn;
        [SerializeField] private Button _leaderboardBtn;
        [SerializeField] private Button _logoutBtn;

        [Header("Registration")]
        [SerializeField] private TMP_InputField _username;
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private Button _registerSubmitBtn;
        [SerializeField] private Button _registerCancelBtn;

        private Profile _profile;
        private bool _register = false;
        
        public void Initialize(Profile profile)
        {
            _profile = profile;
            _profile.Registered += SetRegistered;
            _profile.Unregistered += SetUnregistered;
            
            SetObjectsActive(false, false, false, false);
            
            _menuBtn.onClick.AddListener(OnMenuClicked);
            _registerBtn.onClick.AddListener(OnRegisterClicked);
            _loginBtn.onClick.AddListener(OnLogInClicked);
            _leaderboardBtn.onClick.AddListener(OnLeaderboardClicked);
            _logoutBtn.onClick.AddListener(OnLogoutClicked);
            _registerSubmitBtn.onClick.AddListener(OnSubmitClicked);
            _registerCancelBtn.onClick.AddListener(SetUnregistered);

            if (_menu.activeSelf)
                OnMenuClicked();
        }

        private void OnMenuClicked()
        {
            _menu.gameObject.SetActive(!_menu.gameObject.activeSelf);
        }

        private void SetObjectsActive(bool unregistered, bool registered, bool register, bool username)
        {
            _registerBtn.gameObject.SetActive(unregistered);
            _loginBtn.gameObject.SetActive(unregistered);
            
            _leaderboardBtn.gameObject.SetActive(registered);
            _logoutBtn.gameObject.SetActive(registered);
            
            _email.gameObject.SetActive(register);
            _password.gameObject.SetActive(register);
            _registerSubmitBtn.gameObject.SetActive(register);
            _registerCancelBtn.gameObject.SetActive(register);
            
            _username.gameObject.SetActive(username);
        }

        public void SetUnregistered()
        {
            _text.text = "Unregistered";
            SetObjectsActive(true, false, false, false);
        }

        public void SetRegistered()
        {
            var acc = StaticParameters.Account;
            _text.text = acc.Name;
            SetObjectsActive(false, true, false, false);
        }

        private void OnRegisterClicked()
        {
            _register = true;
            SetObjectsActive(false, false, true, true);
        }

        private void OnLogInClicked()
        {
            _register = false;
            SetObjectsActive(false, false, true, false);
        }

        private void OnLeaderboardClicked()
        {
            
        }

        private void OnLogoutClicked()
        {
            StaticParameters.Account = new(string.Empty, string.Empty);
            PlayerPrefs.SetString(Profile.PlayerPrefPassword, string.Empty);
            PlayerPrefs.SetString(Profile.PlayerPrefEmail, string.Empty);
            PlayerPrefs.Save();
            SetUnregistered();
        }

        private void OnSubmitClicked()
        {
            string username = _username.text;
            string email = _email.text;
            string password = _password.text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                email.Length > 80 || password.Length > 30)
                return;

            if (_register)
            {
                if (string.IsNullOrEmpty(username) ||
                    string.IsNullOrWhiteSpace(username) ||
                    username.Length > 15)
                    return;

                _text.text = "...";
                SetObjectsActive(false, false, false, false);
                _profile.Register(email, password, username);
                return;
            }

            _text.text = "...";
            SetObjectsActive(false, false, false, false);
            _profile.LogIn(email, password);
        }
    }
}