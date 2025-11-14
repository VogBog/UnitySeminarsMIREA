namespace Game
{
    public interface IGameFinisher
    {
        CarMapRunner[] GetRunnersForRecord();
        void OnBeforeLoadingScene();
    }
}