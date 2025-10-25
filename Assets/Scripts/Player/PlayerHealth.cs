using System;
using Damage;
using MainGame;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerHealth : IDamageable
    {
        public const int MaxHealth = 4;

        private bool _died = false;
        private Player _player;
        private EventBus _eventBus;
        
        public int Health { get; private set; }

        public void Initialize(EventBus eventBus, Player player)
        {
            Health = MaxHealth;
            _eventBus = eventBus;
            _player = player;
            
            _eventBus.InvokePlayerHealthChanged(_player, this, Health);
        }
        
        public virtual void TakeDamage(ref GetDamageData data)
        {
            if (data.Damage <= 0 || _died)
                return;
            
            Health = Mathf.Clamp(Health - data.Damage, 0, MaxHealth);
            _eventBus.InvokePlayerTakeDamage(_player, ref data);
            _eventBus.InvokePlayerHealthChanged(_player, this, Health);
            
            if (Health == 0)
            {
                _died = true;
                _eventBus.InvokePlayerDied(_player);

                if (data.Attacker.TryGetComponent<Player>(out var killer))
                {
                    _eventBus.InvokePlayerKilledByAnotherPlayer(this, killer);
                }
            }
        }
    }
}