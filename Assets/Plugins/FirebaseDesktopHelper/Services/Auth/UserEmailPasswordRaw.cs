using System;

namespace FirebaseDesktopHelper.Services.Auth
{
    [Serializable]
    public struct UserEmailPasswordRaw
    {
        public string email;
        public string password;
        public bool returnSecureToken;

        public UserEmailPasswordRaw(UserEmailPassword userEmailPassword)
        {
            email = userEmailPassword.Email;
            password = userEmailPassword.Password;
            returnSecureToken = userEmailPassword.ReturnSecureToken;
        }

        public UserEmailPassword Convert()
        {
            return new UserEmailPassword(email, password, returnSecureToken);
        }
    }
}