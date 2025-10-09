namespace Leaderboard.FirebaseDesktopHelper
{
    public class ReferenceWithFlag<T>
    {
        public bool IsReady { get; private set; } = false;
        public T Value { get; private set; }

        public void Set(T value)
        {
            Value = value;
            IsReady = true;
        }
    }
}