using Game.LocalMultiplayer;
using Global;

namespace Game
{
    public static class GameServices
    {
        public static ICarsSpawner CreateCarsSpawner()
        {
            var lm = UnityEngine.Object.FindFirstObjectByType<LocalMultiplayerCarsSpawner>();
            
            if(StaticParameters.NetworkType is StaticParameters.NetworkTypes.LocalMultiplayer)
                return lm;
            
            UnityEngine.Object.Destroy(lm);
            return new SimpleCarsSpawner();
        }
    }
}