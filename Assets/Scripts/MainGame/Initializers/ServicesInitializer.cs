using System;
using Data;
using MainGame.GameStarters;
using MainMenu;
using Player;
using Player.NetworkPolitics;
using Pool;

namespace MainGame.Initializers
{
    public static class ServicesInitializer
    {
        private static T Find<T>() where T : UnityEngine.Object
            => Extensions.ObjectExtensions.FindFirstObjectByTypeOrException<T>();
        
        public static IGameStarter InitializeServices()
        {
            var result = GetServicesInitializer().InitializeSystems(
                Find<PlayersRepo>(),
                Find<GameTimer>(),
                Find<EventBus>(),
                Find<GameFinisher>(),
                Find<ObjectPool>(),
                Find<GridMap.GridMap>());
            
            Find<LocalMultiplayerGameStarter>().DestroyIfNotLocalMultiplayer();

            return result;
        }
        
        public static INetworkPolitics GetPlayerPolitics(PlayerHealth playerHealth)
        {
            return GetServicesInitializer().GetPlayerPolitics(playerHealth);
        }

        public static IServicesInitializer GetServicesInitializer()
        {
            return StaticParameters.NetworkType switch
            {
                NetworkTypes.SplitScreen => new SplitScreenInitializer(),
                NetworkTypes.LocalMultiplayer => new LocalMultiplayerInitializer(),
                _ => throw new NotImplementedException()
            };
        }
    }
}