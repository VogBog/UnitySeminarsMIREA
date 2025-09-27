using System;
using System.Collections;
using Damage;
using Data;
using Extensions;
using GridMap;
using Pool;
using UnityEngine;

namespace Player.Abilities.Ice
{
    public class IceProjectile : MonoBehaviour
    {
        private ObjectPool _pool;
        private Player _player;
        private GridMap.GridMap _gridMap;
        private Coroutine _lifetimeCor;
        private ProjectileParticles _particles;

        private bool _exploded;
        private int _damage;
        private float _speed;
        private float _distance;
        private float _explodeRadius;
        
        public void SetPool(ObjectPool pool, Player player)
        {
            _pool = pool;
            _player = player;
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap.GridMap>();
        }

        public void ThrowForward(int damage, float speed, float distance, float explodeRadius)
        {
            _damage = damage;
            _speed = speed;
            _distance = distance;
            _explodeRadius = explodeRadius;
            _exploded = false;
            _particles = _pool.Spawn<IceProjectileParticles>(transform.position, Quaternion.identity);
            _particles.Connect(transform);

            _lifetimeCor = StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_distance / _speed);

            Explode();
        }

        public void Explode()
        {
            if(_lifetimeCor != null)
                StopCoroutine(_lifetimeCor);
            if (_exploded)
                return;
            _exploded = true;
            
            _particles.Stop();

            var colliders = Physics.OverlapSphere(transform.position, _explodeRadius);
            foreach (var collider in colliders)
            {
                if(collider.gameObject == _player.HurtBox.gameObject ||
                   !collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
                    continue;

                var damageData = new GetDamageData(_damage, _player.gameObject, Elementals.Ice);
                damageable.TakeDamage(damageData);
            }

            var rects = _gridMap.FromWorldSphereToIndexesSphere(
                transform.position.x, transform.position.z, _explodeRadius, GridMapValues.IceFloor);
            _gridMap.SetCellsAsync(rects);
            
            _pool.Despawn(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject != _player.gameObject)
                Explode();
        }

        private void FixedUpdate()
        {
            transform.position += _speed * Time.fixedDeltaTime * transform.forward;
        }
    }
}