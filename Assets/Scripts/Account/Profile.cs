using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Account
{
    public class Profile : MonoBehaviour
    {
        [SerializeField] private ProfileMenu _menu;
        
        public const string PlayerPrefEmail = "Email";
        public const string PlayerPrefPassword = "Password";

        public event Action Unregistered;
        public event Action Registered; 
        
        private void Start()
        {
            _menu.Initialize(this);
            
            string email = PlayerPrefs.GetString(PlayerPrefEmail);
            string password = PlayerPrefs.GetString(PlayerPrefPassword);

            LogIn(email, password);
        }

        public void LogIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Unregistered?.Invoke();
                return;
            }
            
            PlayerPrefs.SetString(PlayerPrefEmail, email);
            PlayerPrefs.SetString(PlayerPrefPassword, password);
            
            AccountRepository.LogIn(email, password, res =>
            {
                if (res == UnityWebRequest.Result.Success)
                {
                    Registered?.Invoke();
                    return;
                }
                
                AccountRepository.LogIn(email, password, res2 =>
                {
                    if (res2 == UnityWebRequest.Result.Success)
                    {
                        Unregistered?.Invoke();
                        return;
                    }
                    
                    AccountRepository.LogIn(email, password, res3 =>
                    {
                        if (res3 == UnityWebRequest.Result.Success)
                        {
                            Registered?.Invoke();
                        }
                        else
                        {
                            Unregistered?.Invoke();
                            PlayerPrefs.SetString(PlayerPrefEmail, string.Empty);
                            PlayerPrefs.SetString(PlayerPrefPassword, string.Empty);
                        }
                    });
                });
            });
        }

        public void Register(string email, string password, string username)
        {
            PlayerPrefs.SetString(PlayerPrefEmail, email);
            PlayerPrefs.SetString(PlayerPrefPassword, password);
            
            AccountRepository.Register(email, password, username, res =>
            {
                if (res == UnityWebRequest.Result.Success)
                {
                    Registered?.Invoke();
                    return;
                }
                
                AccountRepository.Register(email, password, username, res2 =>
                {
                    if (res == UnityWebRequest.Result.Success)
                    {
                        Registered?.Invoke();
                        return;
                    }
                    
                    Unregistered?.Invoke();
                });
            });
        }
    }
}