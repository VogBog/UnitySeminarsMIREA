using FirebaseDesktopHelper.CompleterChoosers;
using FirebaseDesktopHelper.CompletersAsync;

namespace FirebaseDesktopHelper
{
    public readonly struct FirebaseRestQueryCompleter
    {
        private readonly FirebaseRestQuery _query;

        public FirebaseRestQueryCompleter(FirebaseRestQuery query)
        {
            _query = query;
        }

        public IFirebaseQueryCompleterChooser Get() =>
            new FirebaseQueryCompleterDefaultChooser(new FirebaseQueryCompleterGetAsync(_query));
        
        public IFirebaseQueryCompleterChooser Post() =>
            new FirebaseQueryCompleterDefaultChooser(new FirebaseQueryCompleterPostAsync(_query));
        
        public IFirebaseQueryCompleterChooser Put() =>
            new FirebaseQueryCompleterDefaultChooser(new FirebaseQueryCompleterPutAsync(_query));
        
        public IFirebaseQueryCompleterChooser Delete() =>
            new FirebaseQueryCompleterDefaultChooser(new FirebaseQueryCompleterDeleteAsync(_query));
    }
}