using System;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    [Serializable]
    public struct SignInResponseRaw
    {
        public string localId;
        public string idToken;
        public string refreshToken;
        public float expiresIn;

        public SignInResponse Convert()
        {
            return new SignInResponse()
            {
                LocalId = localId,
                IdToken = idToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn
            };
        }
    }
}