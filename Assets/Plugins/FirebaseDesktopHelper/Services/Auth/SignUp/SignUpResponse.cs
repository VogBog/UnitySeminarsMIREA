using System;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    [Serializable]
    public struct SignUpResponse
    {
        public string LocalId;
        public string IdToken;
        public string RefreshToken;
        public float ExpiresIn;
    }
}