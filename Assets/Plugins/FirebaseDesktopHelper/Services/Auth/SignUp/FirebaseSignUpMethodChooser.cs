using Newtonsoft.Json;
using UnityEngine;

namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    public struct FirebaseSignUpMethodChooser
    {
        public FirebaseSignUpChooser FromEmailPassword(
            string email, string password, bool returnSecureToken = true)
        {
            return FromEmailPassword(new UserEmailPassword(email, password, returnSecureToken));
        }

        public FirebaseSignUpChooser FromEmailPassword(UserEmailPassword userEmailPassword)
        {
            string json = JsonConvert.SerializeObject(new UserEmailPasswordRaw(userEmailPassword));
            return new FirebaseSignUpChooser(json);
        }

        public FirebaseSignUpChooser FromJson(string json)
        {
            return new FirebaseSignUpChooser(json);
        }
    }
}