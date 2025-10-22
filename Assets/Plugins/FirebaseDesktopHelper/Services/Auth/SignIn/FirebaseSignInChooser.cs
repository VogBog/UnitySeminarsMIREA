namespace FirebaseDesktopHelper.Services.Auth.SignIn
{
    public readonly struct FirebaseSignInChooser
    {
        private readonly string _json;

        public FirebaseSignInChooser(string json)
        {
            _json = json;
        }
        
        public FirebaseSignInAsync Async() => new FirebaseSignInAsync(_json);
        public FirebaseSignInCallback Callback() => new FirebaseSignInCallback(_json);
        public FirebaseSignInCoroutine Coroutine() => new FirebaseSignInCoroutine(_json);
        public FirebaseSignInVoid Void() => new FirebaseSignInVoid(Async());
    }
}