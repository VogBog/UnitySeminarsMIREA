using Pool;
using UnityEngine;

namespace Player.Abilities.Thunder
{
    public class ThunderAbility : IPlayerAbility
    {
        private readonly Player _player;
        private readonly ObjectPool _pool;
        private readonly ThunderAbilityData _data;

        public IPlayerAbilityData Data => _data;

        public ThunderAbility(Player player, ThunderAbilityData data)
        {
            _player = player;
           _data = data;

            _pool = Extensions.ObjectExtensions.FindFirstObjectByTypeOrException<ObjectPool>();
            
            _pool.RegisterAndInstantiatePrefab(typeof(ThunderProjectile),
                new(
                    data.Projectile,
                    (pool, comp) => (comp as ThunderProjectile)?.SetPool(pool), 
                    null),
                2);
        }
        
        public void QuickAbility()
        {
            var projectile = _pool.Spawn<ThunderProjectile>(_player.RealTransform.position, Quaternion.identity);
            projectile.Attack(_player, _data, _player.Movement.Forward);
        }

        public void HeavyAbility()
        {
            var projectile = _pool.Spawn<ThunderProjectile>(_player.RealTransform.position, Quaternion.identity);
            projectile.HeavyAttack(_player, _data, _player.Movement.Forward);
        }
    }
}