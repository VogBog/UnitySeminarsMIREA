using Pool;
using UnityEngine;

namespace Player.Abilities.Ice
{
    public class IceAbility : IPlayerAbility
    {
        private readonly Player _player;
        private readonly IceAbilityData _data;
        private readonly ObjectPool _pool;
        
        public IPlayerAbilityData Data => _data;

        public IceAbility(Player player, IceAbilityData data)
        {
            _player = player;
            _data = data;
            _pool = Extensions.ObjectExtensions.FindFirstObjectByTypeOrException<ObjectPool>();
            
            _pool.RegisterAndInstantiatePrefab(typeof(IceProjectile),
                new(data.ProjectilePrefab,
                    (pool, comp) => (comp as IceProjectile)?.SetPool(pool, _player),
                    null),
                3);
            
            _pool.RegisterAndInstantiatePrefab(typeof(IceWall),
                new(data.WallPrefab,
                    (pool, comp) => (comp as IceWall)?.SetPool(pool),
                    null),
                3);
        }
        
        public void QuickAbility()
        {
            var projectile = _pool.Spawn<IceProjectile>(
                _player.AbilityUsage.ProjectileOrigin.position,
                Quaternion.LookRotation(_player.Movement.Forward));
            
            projectile.ThrowForward(
                _data.QuickDamage, _data.QuickSpeed, _data.QuickDistance, _data.QuickExplosionRadius);
        }

        public void HeavyAbility()
        {
            var wall = _pool.Spawn<IceWall>(
                _player.AbilityUsage.ProjectileOrigin.position,
                Quaternion.LookRotation(_player.Movement.Forward));

            wall.transform.position += wall.transform.forward;
            
            wall.SetWall(_data.WallHealth, _data.WallLifetime);
        }
    }
}