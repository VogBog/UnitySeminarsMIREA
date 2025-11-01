using MainGame.GameStarters;
using Player;
using Player.NetworkPolitics;
using Pool;

namespace MainGame.Initializers
{
    public interface IServicesInitializer
    {
        IGameStarter InitializeSystems(
            PlayersRepo playersRepo,
            GameTimer gameTimer,
            EventBus eventBus,
            GameFinisher gameFinisher,
            ObjectPool pool,
            GridMap.GridMap gridMap);
        
        INetworkPolitics GetPlayerPolitics(PlayerHealth playerHealth);
    }
}