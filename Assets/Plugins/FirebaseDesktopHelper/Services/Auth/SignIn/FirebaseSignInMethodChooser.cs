using Newtonsoft.Json;

namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    public struct FirebaseSignInMethodChooser
    {
        public FirebaseSignInChooser FromEmailPassword(
            string email, string password, bool returnSecureToken = true)
        {
            return FromEmailPassword(new UserEmailPassword(email, password, returnSecureToken));
        }

        public FirebaseSignInChooser FromEmailPassword(UserEmailPassword userEmailPassword)
        {
            string json = JsonConvert.SerializeObject(new UserEmailPasswordRaw(userEmailPassword));
            return new FirebaseSignInChooser(json);
        }

        public FirebaseSignInChooser FromJson(string json)
        {
            return new FirebaseSignInChooser(json);
        }
    }
}