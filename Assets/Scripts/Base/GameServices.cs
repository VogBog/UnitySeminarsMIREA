using Game.GameStarter;
using UnityEngine;

namespace Base
{
    public static class GameServices
    {
        public static IGameStarter CreateGameStarter()
        {
            var lm = Object.FindFirstObjectByType<LocalMultiplayerGameStarter>();
            
            if(StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
                Object.Destroy(lm);
            
            return lm;
        }
    }
}