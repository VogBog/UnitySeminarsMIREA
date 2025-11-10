using GridMap.NetworkPolitics;
using MainGame.EventBuses;
using MainGame.GameFinishers;
using MainGame.GameStarters;
using MainGame.GameTimers;
using MainGame.NetworkMessaging;
using MainGame.PlayersRepos;
using Player;
using Player.NetworkPolitics;
using Pool;

namespace MainGame.Initializers
{
    public class LocalMultiplayerInitializer : IServicesInitializer
    {
        public IGameStarter InitializeSystems(PlayersRepo playersRepo, GameTimer gameTimer, EventBus eventBus,
            GameFinisher gameFinisher, ObjectPool pool, GridMap.GridMap gridMap)
        {
            var localMpStarter =
                Extensions.ObjectExtensions.FindFirstObjectByTypeOrException<LocalMultiplayerGameStarter>();

            var localMultiplayerPlayersRepo = localMpStarter.GetComponent<LocalMultiplayerPlayersRepo>();
            playersRepo.Initialize(localMultiplayerPlayersRepo);

            var lmGameTime = localMpStarter.GetComponent<LocalMultiplayerGameTimer>();
            gameTimer.Initialize(lmGameTime);

            var messages = localMpStarter.GetComponent<LocalMultiplayerMessages>();
            eventBus.Initialize(new LocalMultiplayerEventBus(messages));

            var lmGameFinisher = localMpStarter.GetComponent<LocalMultiplayerGameFinisher>();
            gameFinisher.Initialize(lmGameFinisher);

            pool.SetPool(new LocalMultiplayerObjectPoolDecorator(
                new DefaultObjectPool(pool.transform, pool),
                localMpStarter.GetComponent<LocalMultiplayerObjectPoolNetworkObject>()));

            var lmGridMap = localMpStarter.GetComponent<LocalMultiplayerGridMap>();
            lmGridMap.Initialize(new DefaultGridMap(gridMap));
            gridMap.Initialize(lmGridMap);

            return localMpStarter;
        }

        public INetworkPolitics GetPlayerPolitics(PlayerHealth playerHealth)
        {
            var politics = playerHealth.Owner.transform.GetComponent<LocalMultiplayerNetworkPolitics>();
            politics.SetPlayer(playerHealth.Owner);
            
            return politics;
        }
    }
}