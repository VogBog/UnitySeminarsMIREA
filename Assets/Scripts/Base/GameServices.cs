using Game.FruitsSpawner;
using Game.GameFinisher;
using Game.GameStarter;
using Game.Player;
using Game.PlayerKillers;
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

        public static ISnakeTail CreateSnakeTail(SnakeTail tail)
        {
            var lm = tail.GetComponent<LocalMultiplayerSnakeTail>();
            
            if(StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
                Object.Destroy(lm);

            return lm;
        }

        public static IPlayerKiller CreatePlayerKiller()
        {
            var lm = Object.FindFirstObjectByType<LocalMultiplayerPlayerKiller>();
            
            if(StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
                Object.Destroy(lm);
            
            return lm;
        }

        public static IGameFinisher CreateGameFinisher()
        {
            var lm = Object.FindFirstObjectByType<LocalMultiplayerGameFinisher>();
            
            if(StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
                Object.Destroy(lm);
            
            return lm;
        }

        public static IFruitsSpawner CreateFruitsSpawner()
        {
            var lm = Object.FindFirstObjectByType<LocalMultiplayerFruitsSpawner>();
            
            if(StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
                Object.Destroy(lm);
            
            return lm;
        }
    }
}