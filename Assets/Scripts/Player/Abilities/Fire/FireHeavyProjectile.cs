using System.Collections;
using Damage;
using Data;
using GridMap;
using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    public class FireHeavyProjectile : MonoBehaviour
    {
        private Player _player;
        private ObjectPool _pool;
        private GridMap.GridMap _gridMap;
        private ProjectileParticles _particles;
        
        private int _damage;
        private float _speed;
        private float _yDistance;
        private float _distance;
        private float _radius;

        private bool _moveDown = false;

        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
            _gridMap = FindFirstObjectByType<GridMap.GridMap>();
        }

        public void Throw(Player player, FireAbilityData data)
        {
            _player = player;
            _damage = data.HeavyDamage;
            _speed = data.HeavySpeed;
            _yDistance = data.HeavyDistance;
            _distance = data.HeavyDistance;
            _radius = data.HeavyRadius;
            _pool.Spawn<FireHeavyProjectileParticles>(transform.position, Quaternion.identity, particles =>
            {
                _particles = particles;
                _particles.Connect(transform);
                
                transform.position += Vector3.up * _yDistance;
                StartCoroutine(LifetimeRoutine());
            });
        }

        private IEnumerator LifetimeRoutine()
        {
            _moveDown = true;
            yield return new WaitForSeconds(_yDistance / _speed);
            _moveDown = false;

            Explode();
        }

        public void Explode()
        {
            _particles.Stop();
            
            _pool.Spawn<ExplosionEffect>(transform.position, Quaternion.identity, effect =>
            {
                effect.Explode();
            });
            
            var colliders = Physics.OverlapSphere(transform.position, _radius);
            foreach (var collider in colliders)
            {
                if(collider.gameObject == _player.HurtBox.gameObject ||
                   !collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
                    continue;

                var data = new GetDamageData(_damage, _player.gameObject, Elementals.Fire, true);
                damageable.TakeDamage(ref data);
            }

            var rects = _gridMap.FromWorldSphereToIndexesSphere(
                transform.position.x, transform.position.z, _radius, GridMapValues.Fire10Seconds);
            _gridMap.SetCellsAsync(rects);
            
            _pool.Despawn(this);
        }

        private void FixedUpdate()
        {
            if(_moveDown)
                transform.position += _speed * Time.fixedDeltaTime * Vector3.down;
        }
    }
}