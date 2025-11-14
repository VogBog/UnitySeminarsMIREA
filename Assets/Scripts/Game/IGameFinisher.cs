namespace Game
{
    public interface IGameFinisher
    {
        CarMapRunner[] GetRunnersForRecord();
        bool CanFinish();
        void OnBeforeLoadingScene();
    }
}