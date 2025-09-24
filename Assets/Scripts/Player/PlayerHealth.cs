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

        public event Action<PlayerHealth> Died; 
        
        public int Health { get; private set; }

        public void Initialize()
        {
            Health = MaxHealth;
        }
        
        public virtual void TakeDamage(GetDamageData data)
        {
            if (data.Damage <= 0 || _died)
                return;
            
            Health = Mathf.Clamp(Health - data.Damage, 0, MaxHealth);
            if (Health == 0)
            {
                _died = true;
                Died?.Invoke(this);
            }
        }
    }
}