using System;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    [Serializable]
    public struct SignInResponse
    {
        public string LocalId;
        public string IdToken;
        public string RefreshToken;
        public float ExpiresIn;
    }
}