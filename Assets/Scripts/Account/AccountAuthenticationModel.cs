using System.Collections;
using Data;
using FirebaseDesktopHelper;
using FirebaseDesktopHelper.Services.Auth.SignIn;
using FirebaseDesktopHelper.Services.Auth.SignUp;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Account
{
    public class AccountAuthenticationModel : MonoBehaviour
    {
        [SerializeField] private AccountWelcomeScreen _welcomeScreen;
        
        public const string PlayerPrefsEmail = "Email";
        public const string PlayerPrefsPassword = "Password";

        private void Awake()
        {
            _welcomeScreen.SignUpNicknameEmailPassword += SignUp;
            _welcomeScreen.LogInEmailPassword += SignIn;
            _welcomeScreen.SinglePlayerChoose += GoSinglePlayer;
            
            _welcomeScreen.Initialize();
        }
        
        private IEnumerator Start()
        {
            var boolRef = new Reference<bool>();
            var signInRef = new Reference<(SignInResponse, UnityWebRequest.Result)>();
            
            yield return AccountAuthenticationRoutines.SeveralTries(
                AccountAuthenticationRoutines.TrySignInRoutine((b, resp, status) =>
                {
                    boolRef.Value = b;
                    signInRef.Value = (resp, status);
                }),
                () => signInRef.Value.Item2);

            if (signInRef.Value.Item2 != UnityWebRequest.Result.Success)
            {
                _welcomeScreen.ShowErrorText("Cannot sign in.");
                _welcomeScreen.ShowWelcomeScreen();
                yield break;
            }

            if (boolRef.Value)
            {
                yield return GoAuthorizedRoutine(signInRef.Value.Item1.LocalId);
                yield break;
            }

            _welcomeScreen.ShowWelcomeScreen();
        }

        public void SignUp(string nickName, string email, string password)
        {
            StartCoroutine(SignUpRoutine(email, password, nickName));
        }

        private IEnumerator SignUpRoutine(string email, string password, string nickName)
        {
            var isNickNameFreeRef = new ReferenceWithFlag<bool>();
            var isConnectionOkRef = new ReferenceWithFlag<bool>();

            yield return AccountAuthenticationRoutines.IsNickNameFreeRoutineSeveralTries(
                nickName,
                isNickNameFreeRef.Set,
                () => isConnectionOkRef.Set(false));

            if (isConnectionOkRef.IsReady)
            {
                _welcomeScreen.ShowAuthScreen();
                _welcomeScreen.ShowErrorText("Connection error");
                yield break;
            }

            if (!isNickNameFreeRef.Value)
            {
                _welcomeScreen.ShowAuthScreen();
                _welcomeScreen.ShowErrorText("Username is already exists");
                yield break;
            }
                
            var boolRef = new Reference<bool>();
            var signUpRef = new Reference<(SignUpResponse, UnityWebRequest.Result)>();
                
            yield return AccountAuthenticationRoutines.SeveralTries(
                AccountAuthenticationRoutines.TrySignUpRoutine(
                    email, password, nickName, (b, resp, status) =>
                    {
                        boolRef.Value = b;
                        signUpRef.Value = (resp, status);
                    }),
                () => signUpRef.Value.Item2);
            
            if (signUpRef.Value.Item2 != UnityWebRequest.Result.Success)
            {
                _welcomeScreen.ShowErrorText("Connection failed");
                _welcomeScreen.ShowAuthScreen();
                yield break;
            }

            if (!boolRef.Value)
            {
                _welcomeScreen.ShowAuthScreen();
                _welcomeScreen.ShowErrorText("Cannot sign up. Maybe your email is already in use");
                yield break;
            }

            yield return GoAuthorizedRoutine(signUpRef.Value.Item1.LocalId);
        }

        public void SignIn(string email, string password)
        {
            StartCoroutine(SignInRoutine(email, password));
        }

        private IEnumerator SignInRoutine(string email, string password)
        {
            Debug.Log($"Sign in with {email} and {password}");
            
            var boolRef = new Reference<bool>();
            var signInRef = new Reference<(SignInResponse, UnityWebRequest.Result)>();
            
            yield return AccountAuthenticationRoutines.SeveralTries(
                AccountAuthenticationRoutines.TrySignInRoutine(
                    email, password, (b, resp, status) =>
                    {
                        boolRef.Value = b;
                        signInRef.Value = (resp, status);
                    }),
                () => signInRef.Value.Item2);
            
            if (signInRef.Value.Item2 != UnityWebRequest.Result.Success)
            {
                _welcomeScreen.ShowErrorText("Connection failed");
                _welcomeScreen.ShowAuthScreen();
                yield break;
            }

            if (!boolRef.Value)
            {
                _welcomeScreen.ShowAuthScreen();
                _welcomeScreen.ShowErrorText("Invalid email or password");
                yield break;
            }

            yield return GoAuthorizedRoutine(signInRef.Value.Item1.LocalId);
        }

        private IEnumerator GoAuthorizedRoutine(string uuid)
        {
            var playerRef = new Reference<(PlayerAccount, UnityWebRequest.Result)>();
            yield return AccountAuthenticationRoutines.SeveralTries(
                AccountAuthenticationRoutines.GetPlayerRoutine(uuid, (player, status) =>
                    playerRef.Value = (player, status)),
                () => playerRef.Value.Item2);
            
            if (playerRef.Value.Item2 != UnityWebRequest.Result.Success)
            {
                _welcomeScreen.ShowErrorText("Connection failed");
                _welcomeScreen.ShowAuthScreen();
                yield break;
            }
            
            if(!string.IsNullOrEmpty(playerRef.Value.Item1.NickName))
                GoAuthorized(playerRef.Value.Item1);
            else
            {
                _welcomeScreen.ShowErrorText("Something went wrong. Cannot get player");
                _welcomeScreen.ShowAuthScreen();
            }
        }

        private void GoSinglePlayer()
        {
            StaticParameters.SinglePlayer = true;
            GoToMainMenu();
        }

        private void GoAuthorized(PlayerAccount account)
        {
            StaticParameters.PlayerAccount = account;
            StaticParameters.SinglePlayer = false;
            GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene(1);
        }
    }
}