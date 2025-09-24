using Extensions;
using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    public class FireAbility : IPlayerAbility
    {
        private readonly FireAbilityData _data;
        private readonly Player _player;
        private readonly ObjectPool _pool;

        public IPlayerAbilityData Data => _data;

        public FireAbility(Player player, FireAbilityData data)
        {
            _player = player;
            _data = data;
            _pool = ObjectExtensions.FindFirstObjectByTypeOrException<ObjectPool>();
            
            _pool.RegisterAndInstantiatePrefab(
                typeof(FireProjectile),
                new (_data.ProjectilePrefab,
                    (pool, comp) => (comp as FireProjectile)?.SetPool(pool),
                    null),
                4);
            
            _pool.RegisterAndInstantiatePrefab(
                typeof(FireHeavyProjectile),
                new(_data.HeavyProjectilePrefab,
                    (pool, comp) => (comp as FireHeavyProjectile)?.SetPool(pool),
                    null),
                1);
        }

        public void QuickAbility()
        {
            var origin = _player.AbilityUsage.ProjectileOrigin;
            var projectile = _pool.Spawn<FireProjectile>(origin.position, Quaternion.identity);
            projectile.transform.LookAt(projectile.transform.position + _player.Movement.Forward);
            projectile.Throw(_data.QuickSpeed, _data.QuickDistance, _data.QuickDamage, _player.gameObject);
        }

        public void HeavyAbility()
        {
            var pos = _player.RealTransform.position + _player.Movement.Forward * _data.HeavyDistance;
            var projectile = _pool.Spawn<FireHeavyProjectile>(pos, Quaternion.identity);
            projectile.Throw(_player, _data);
        }
    }
}