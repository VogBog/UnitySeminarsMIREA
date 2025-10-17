using FirebaseDesktopHelper;

namespace Data
{
    public static class FirebaseKeys
    {
        public const string ProjectRoot = "ElementalsRoot";
        public const string FirebaseAccounts = "Accounts";

        public static FirebaseRestQuery GetAccounts(this FirebaseRestQuery query)
            => query.GetChild(ProjectRoot).GetChild(FirebaseAccounts);
    }
}