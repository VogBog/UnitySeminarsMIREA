using System.Collections;
using Damage;
using Data;
using Extensions;
using GridMap;
using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    public class FireProjectile : MonoBehaviour
    {
        private float _speed;
        private int _damage;
        private float _quickFireRadius;
        
        private Coroutine _lifetimeCor;
        private ObjectPool _pool;
        private GridMap.GridMap _gridMap;
        private GameObject _attacker;
        private FireProjectileParticles _particles;

        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap.GridMap>();
        }
        
        public void Throw(float speed, float distance, int damage, float quickFireRadius, GameObject attacker)
        {
            _lifetimeCor = StartCoroutine(LifetimeRoutine(distance / speed));
            
            _damage = damage;
            _speed = speed;
            _attacker = attacker;
            _quickFireRadius = quickFireRadius;

            _pool.Spawn<FireProjectileParticles>(transform.position, Quaternion.identity, particles =>
            {
                _particles = particles;
                _particles.Connect(transform);
            });
        }

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * transform.forward;
        }

        private IEnumerator LifetimeRoutine(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);

            Break();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                var data = new GetDamageData(_damage, _attacker, Elementals.Fire, false);
                damageable.TakeDamage(ref data);
            }
            Break();
        }

        public void Break()
        {
            var rects = _gridMap.FromWorldSphereToIndexesSphere(
                transform.position.x, transform.position.z, _quickFireRadius, GridMapValues.QuickFire);
            _gridMap.SetCellsAsync(rects);
            
            _particles.Stop();
            _pool.Despawn(this);
            
            if(_lifetimeCor != null)
                StopCoroutine(_lifetimeCor);
        }
    }
}