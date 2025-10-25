using MainGame.GameStarters;
using Player;
using Player.NetworkPolitics;

namespace MainGame.Initializers
{
    public interface IServicesInitializer
    {
        IGameStarter InitializeSystems(
            PlayersRepo playersRepo,
            GameTimer gameTimer,
            EventBus eventBus,
            GameFinisher gameFinisher);
        
        INetworkPolitics GetPlayerPolitics(PlayerHealth playerHealth);
    }
}