using System.Linq;
using Game.FruitsSpawner;
using Game.GameFinisher;
using Game.GameStarter;
using Game.Player;
using Game.PlayerKillers;
using MultiNetwork;
using UnityEngine;

namespace Base
{
    public static class GameServices
    {
        public static IGameStarter CreateGameStarter() => GetMultiNetworkService<IGameStarter>();

        public static ISnakeTail CreateSnakeTail(SnakeTail tail)
        {
            var services = tail.GetComponents<ISnakeTail>().OfType<Object>().ToArray();
            return GetMultiNetworkService<ISnakeTail>(services);
        }

        public static IPlayerKiller CreatePlayerKiller() => GetMultiNetworkService<IPlayerKiller>();

        public static IGameFinisher CreateGameFinisher() => GetMultiNetworkService<IGameFinisher>();

        public static IFruitsSpawner CreateFruitsSpawner() => GetMultiNetworkService<IFruitsSpawner>();

        public static T GetMultiNetworkService<T>() where T : INetworkTypeRequirer
        {
            var services = Object.FindObjectsByType(
                typeof(Object), FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(x => x is T)
                .ToArray();
            
            return GetMultiNetworkService<T>(services);
        }

        public static T GetMultiNetworkService<T>(Object[] services) where T : INetworkTypeRequirer
        {
            var networkType = StaticParameters.NetworkType;

            for (int i = 0; i < services.Length; i++)
            {
                if (services[i] is not INetworkTypeRequirer requirer ||
                    requirer.RequiredNetworkType != networkType)
                {
                    Object.Destroy(services[i]);
                    services[i] = null;
                }
            }

            return services.OfType<T>().FirstOrDefault();
        }
    }
}