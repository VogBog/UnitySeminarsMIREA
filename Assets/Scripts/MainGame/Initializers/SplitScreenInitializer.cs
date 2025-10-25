using MainGame.EventBuses;
using MainGame.GameFinishers;
using MainGame.GameStarters;
using MainGame.GameTimers;
using MainGame.PlayersRepos;
using Player;
using Player.NetworkPolitics;

namespace MainGame.Initializers
{
    public class SplitScreenInitializer : IServicesInitializer
    {
        public IGameStarter InitializeSystems(
            PlayersRepo playersRepo,
            GameTimer gameTimer,
            EventBus eventBus,
            GameFinisher gameFinisher)
        {
            playersRepo.Initialize(new DefaultPlayersRepo());
            gameTimer.Initialize(new DefaultGameTimer());
            eventBus.Initialize(new DefaultEventBus());
            gameFinisher.Initialize(new DefaultGameFinisher());

            return new SplitScreenGameStarter();
        }

        public INetworkPolitics GetPlayerPolitics(PlayerHealth playerHealth)
        {
            return new DefaultPlayerPolitic(playerHealth);
        }
    }
}