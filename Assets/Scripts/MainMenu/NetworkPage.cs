using System;
using Account;
using Data;
using FirebaseDesktopHelper;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    [Serializable]
    public class NetworkPage
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerInfo;
        [SerializeField] private Button _quitBtn;
        [SerializeField] private Button _backBtn;
        
        [Header("Edit name")]
        [SerializeField] private Button _editNameButton;
        [SerializeField] private TMP_InputField _editNameInputField;
        [SerializeField] private Button _submitEditNameButton;

        public event Action BackClicked; 
        
        public void Initialize(Button networkPageButton)
        {
            if (StaticParameters.SinglePlayer)
            {
                networkPageButton.gameObject.SetActive(false);
                return;
            }
            
            _quitBtn.onClick.AddListener(QuitAccount);
            _editNameButton.onClick.AddListener(OpenChangePlayerNameScreen);
            _submitEditNameButton.onClick.AddListener(SubmitChangingName);
            _editNameInputField.onSubmit.AddListener(_ => SubmitChangingName());
            
            var playerAccount = StaticParameters.PlayerAccount;
            _playerName.text = playerAccount.NickName;
            _editNameInputField.text = playerAccount.NickName;
            UpdatePlayerInfo(playerAccount);
        }

        public void UpdatePlayerInfo(PlayerAccount account)
        {
            _playerInfo.text = $"Max 1 vs 3 score: {account.Max4Score}{Environment.NewLine}" +
                               $"Max 2 vs 2 score: {account.Max2V2Score}{Environment.NewLine}" +
                               $"Max 1 vs 1 score: {account.Max1V1Score}";
        }

        public void GoBack()
        {
            BackClicked?.Invoke();
        }

        public void QuitAccount()
        {
            PlayerPrefs.DeleteKey(AccountAuthenticationModel.PlayerPrefsEmail);
            PlayerPrefs.DeleteKey(AccountAuthenticationModel.PlayerPrefsPassword);
            FirebaseRestRequests.Tokens.ClearTokens();

            SceneManager.LoadScene(0);
        }

        public void OpenChangePlayerNameScreen()
        {
            _editNameInputField.text = _playerName.text;
            _editNameInputField.gameObject.SetActive(true);
            _submitEditNameButton.gameObject.SetActive(true);
            
            _playerName.gameObject.SetActive(false);
            _editNameButton.gameObject.SetActive(false);
        }

        public void SubmitChangingName()
        {
            string result = _editNameInputField.text;
            
            if (string.IsNullOrEmpty(result) ||
                string.IsNullOrWhiteSpace(result) ||
                result.Length > 25)
            {
                return;
            }
            
            _editNameInputField.gameObject.SetActive(false);
            _submitEditNameButton.gameObject.SetActive(false);
            
            _playerName.gameObject.SetActive(true);
            _editNameButton.gameObject.SetActive(true);

            _playerName.text = "Checking for duplicates...";

            SetPlayerName(result);
        }

        private void SetPlayerName(string playerName)
        {
            var account = StaticParameters.PlayerAccount;
            var setAccount = account;
            setAccount.NickName = playerName;
            
            _editNameButton.StartCoroutine(AccountAuthenticationRoutines.IsNickNameFreeRoutineSeveralTries(
                playerName, isFree =>
                {
                    _editNameButton.StartCoroutine(FirebaseRestRequests.RealtimeDatabase.Query()
                        .GetAccounts()
                        .GetChild(account.Id)
                        .SetJson(setAccount)
                        .Call()
                        .Put()
                        .Coroutine()
                        .RequestResult(res =>
                        {
                            if (res != UnityWebRequest.Result.Success)
                                _playerName.text = account.NickName;
                            else
                                _playerName.text = playerName;
                        }));
                },
                () =>
                {
                    _playerName.text = account.NickName;
                }));
        }
    }
}