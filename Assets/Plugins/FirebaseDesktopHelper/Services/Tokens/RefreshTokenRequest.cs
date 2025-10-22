using System;

namespace FirebaseDesktopHelper.Services.Tokens
{
    [Serializable]
    public struct RefreshTokenRequest
    {
        public string grant_type;
        public string refresh_token;

        public RefreshTokenRequest(string refreshToken)
        {
            grant_type = "refresh_token";
            refresh_token = refreshToken;
        }
    }
}