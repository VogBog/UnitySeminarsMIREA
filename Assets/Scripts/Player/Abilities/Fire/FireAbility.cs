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
        }

        public void QuickAbility()
        {
            var origin = _player.AbilityUsage.ProjectileOrigin;
            var projectile = _pool.Spawn<FireProjectile>(origin.position, Quaternion.identity);
            projectile.transform.LookAt(projectile.transform.position + _player.Movement.Forward);
            projectile.Throw(_data.QuickSpeed, _data.QuickDistance, _data.QuickDamage);
        }

        public void HeavyAbility()
        {
            
        }
    }
}