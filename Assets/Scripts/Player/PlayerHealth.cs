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

        public event Action<PlayerHealth, int> Changed; 
        public event Action<PlayerHealth> Died; 
        
        public int Health { get; private set; }

        public void Initialize(EventBus eventBus, Player player)
        {
            Health = MaxHealth;
            _eventBus = eventBus;
            _player = player;
            Changed?.Invoke(this, Health);
        }
        
        public virtual void TakeDamage(ref GetDamageData data)
        {
            if (data.Damage <= 0 || _died)
                return;
            
            Debug.Log($"Take {data.Damage} damage");
            
            Health = Mathf.Clamp(Health - data.Damage, 0, MaxHealth);
            Changed?.Invoke(this, Health);
            _eventBus.InvokePlayerTakeDamage(_player, ref data);
            
            if (Health == 0)
            {
                _died = true;
                Died?.Invoke(this);

                if (data.Attacker.TryGetComponent<Player>(out var killer))
                {
                    _eventBus.InvokePlayerKilledByAnotherPlayer(this, killer);
                }
            }
        }
    }
}