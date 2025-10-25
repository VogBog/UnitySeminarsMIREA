using System;
using Damage;
using MainGame.EventBuses;
using Player;
using UnityEngine;

namespace MainGame
{
    public class EventBus : MonoBehaviour, IEventBus
    {
        private IEventBus _eventBus;
        
        public delegate void GetDamageDelegate(Player.Player player, ref GetDamageData data);

        public event GetDamageDelegate PlayerTakeDamage;
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer;
        public event Action<Player.Player, PlayerHealth, int> PlayerHealthChanged;
        public event Action<Player.Player> PlayerDied;

        public EventBus Initialize(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.PlayerTakeDamage += InvokePlayerTakeDamageLocal;
            _eventBus.PlayerKilledByAnotherPlayer += (ph, p) => PlayerKilledByAnotherPlayer?.Invoke(ph, p);
            _eventBus.PlayerHealthChanged += (p, ph, i) => PlayerHealthChanged?.Invoke(p, ph, i);
            _eventBus.PlayerDied += p => PlayerDied?.Invoke(p);

            return this;
        }
        
        private void InvokePlayerTakeDamageLocal(Player.Player player, ref GetDamageData data) =>
            PlayerTakeDamage?.Invoke(player, ref data);

        public void InvokePlayerTakeDamage(Player.Player damageReceiver, ref GetDamageData data) 
            => _eventBus.InvokePlayerTakeDamage(damageReceiver, ref data);

        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth health, Player.Player killer)
            => _eventBus.InvokePlayerKilledByAnotherPlayer(health, killer);

        public void InvokePlayerHealthChanged(Player.Player player, PlayerHealth pHealth, int curHealth)
            => _eventBus.InvokePlayerHealthChanged(player, pHealth, curHealth);

        public void InvokePlayerDied(Player.Player player) => _eventBus.InvokePlayerDied(player);
    }
}