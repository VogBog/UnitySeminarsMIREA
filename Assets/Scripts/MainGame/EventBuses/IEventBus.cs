using System;
using Damage;
using Player;

namespace MainGame.EventBuses
{
    public interface IEventBus
    {
        public event EventBus.GetDamageDelegate PlayerTakeDamage;
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer;
        public event Action<Player.Player, PlayerHealth, int> PlayerHealthChanged;
        public event Action<Player.Player> PlayerDied; 

        public void InvokePlayerTakeDamage(Player.Player damageReceiver, ref GetDamageData data);
        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth health, Player.Player killer);
        public void InvokePlayerHealthChanged(Player.Player player, PlayerHealth pHealth, int curHealth);
        public void InvokePlayerDied(Player.Player player);
    }
}