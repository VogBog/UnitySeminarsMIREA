using System;
using Damage;
using MainGame.NetworkMessaging;
using Player;

namespace MainGame.EventBuses
{
    public class LocalMultiplayerEventBus : IEventBus
    {
        public event EventBus.GetDamageDelegate PlayerTakeDamage;
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer;
        public event Action<Player.Player, PlayerHealth, int> PlayerHealthChanged;
        public event Action<Player.Player> PlayerDied;

        private readonly LocalMultiplayerMessages _messages;

        public LocalMultiplayerEventBus(LocalMultiplayerMessages messages)
        {
            _messages = messages;
            messages.PlayerKilledByAnotherPlayer += (pH, p) => PlayerKilledByAnotherPlayer?.Invoke(pH, p);
            messages.PlayerHealthChanged += (p, pH, val) => PlayerHealthChanged?.Invoke(p, pH, val);
            messages.PlayerDied += p => PlayerDied?.Invoke(p);
        }
        
        public void InvokePlayerTakeDamage(Player.Player damageReceiver, ref GetDamageData data)
        {
            PlayerTakeDamage?.Invoke(damageReceiver, ref data);
        }

        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth health, Player.Player killer)
        => _messages.InvokePlayerKilledByAnotherPlayer(health, killer);

        public void InvokePlayerHealthChanged(Player.Player player, PlayerHealth pHealth, int curHealth)
        => _messages.InvokePlayerHealthChanged(player, pHealth, curHealth);

        public void InvokePlayerDied(Player.Player player)
            => _messages.InvokePlayerDied(player);
    }
}