using System;
using System.Collections;

namespace FirebaseDesktopHelper
{
    public interface IFirebaseQueryCompleterIEnumerator
    {
        IEnumerator Empty();
        IEnumerator String(Action<string> callback);
        IEnumerator Object<T>(Action<T> callback);
        IEnumerator ObjectWrapped<T>(string wrapperName, Action<T> callback);
    }
}