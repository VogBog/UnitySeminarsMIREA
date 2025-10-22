using System;

namespace FirebaseDesktopHelper.Services.Tokens
{
    [Serializable]
    public struct RefreshTokenResponse
    {
        public string expires_in;
        public string token_type;
        public string refresh_token;
        public string id_token;
        public string user_id;
        public string project_id;
    }
}