using System;

namespace FirebaseDesktopHelper.Services.Auth
{
    [Serializable]
    public struct UserEmailPassword
    {
        public string Email;
        public string Password;
        public bool ReturnSecureToken;

        public UserEmailPassword(string email, string password, bool returnSecureToken = true)
        {
            Email = email;
            Password = password;
            ReturnSecureToken = returnSecureToken;
        }
    }
}