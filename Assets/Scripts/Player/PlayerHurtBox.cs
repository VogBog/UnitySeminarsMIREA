using Damage;
using UnityEngine;

namespace Player
{
    public class PlayerHurtBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Player _player;
        
        public void TakeDamage(GetDamageData data)
        {
            _player.TakeDamage(data);
        }
    }
}