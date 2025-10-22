namespace FirebaseDesktopHelper.Services.Auth.SignUp
{
    public readonly struct FirebaseSignUpChooser
    {
        private readonly string _json;

        public FirebaseSignUpChooser(string json)
        {
            _json = json;
        }
        
        public FirebaseSignUpAsync Async() => new FirebaseSignUpAsync(_json);
        public FirebaseSignUpCallback Callback() => new FirebaseSignUpCallback(_json);
        public FirebaseSignUpCoroutine Coroutine() => new FirebaseSignUpCoroutine(_json);
        public FirebaseSignUpVoid Void() => new FirebaseSignUpVoid(Async());
    }
}