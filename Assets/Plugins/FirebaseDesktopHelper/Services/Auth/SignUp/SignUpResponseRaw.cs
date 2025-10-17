using System;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    [Serializable]
    public struct SignUpResponseRaw
    {
        public string localId;
        public string idToken;
        public string refreshToken;
        public float expiresIn;

        public SignUpResponse Convert()
        {
            return new()
            {
                LocalId = localId,
                IdToken = idToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn
            };
        }
    }
}