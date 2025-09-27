using System;
using Damage;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerHealth : IDamageable
    {
        public const int MaxHealth = 4;

        private bool _died = false;

        public event Action<PlayerHealth, int> Changed; 
        public event Action<PlayerHealth> Died; 
        
        public int Health { get; private set; }

        public void Initialize()
        {
            Health = MaxHealth;
            Changed?.Invoke(this, Health);
        }
        
        public virtual void TakeDamage(GetDamageData data)
        {
            if (data.Damage <= 0 || _died)
                return;
            
            Debug.Log($"Take {data.Damage} damage");
            
            Health = Mathf.Clamp(Health - data.Damage, 0, MaxHealth);
            Changed?.Invoke(this, Health);
            
            if (Health == 0)
            {
                _died = true;
                Died?.Invoke(this);
            }
        }
    }
}