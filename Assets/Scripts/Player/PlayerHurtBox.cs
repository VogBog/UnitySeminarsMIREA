using Damage;
using UnityEngine;

namespace Player
{
    public class PlayerHurtBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Player _player;
        
        public void TakeDamage(ref GetDamageData data)
        {
            _player.TakeDamage(ref data);
        }
    }
}