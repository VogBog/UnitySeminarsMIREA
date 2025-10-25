using System;
using Damage;
using Player;

namespace MainGame.EventBuses
{
    public class DefaultEventBus : IEventBus
    {
        public event EventBus.GetDamageDelegate PlayerTakeDamage;
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer;
        public event Action<Player.Player, PlayerHealth, int> PlayerHealthChanged;
        public event Action<Player.Player> PlayerDied;

        public void InvokePlayerTakeDamage(Player.Player damageReceiver, ref GetDamageData data)
        {
            PlayerTakeDamage?.Invoke(damageReceiver, ref data);
        }

        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth health, Player.Player killer)
        {
            PlayerKilledByAnotherPlayer?.Invoke(health, killer);
        }

        public void InvokePlayerHealthChanged(Player.Player player, PlayerHealth pHealth, int curHealth)
        {
            PlayerHealthChanged?.Invoke(player, pHealth, curHealth);
        }

        public void InvokePlayerDied(Player.Player player)
        {
            PlayerDied?.Invoke(player);
        }
    }
}