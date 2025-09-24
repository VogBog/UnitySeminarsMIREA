using System.Collections;
using Damage;
using Data;
using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    public class FireProjectile : MonoBehaviour
    {
        private float _speed;
        private int _damage;
        private Coroutine _lifetimeCor;
        private ObjectPool _pool;
        private GameObject _attacker;

        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
        }
        
        public void Throw(float speed, float distance, int damage, GameObject attacker)
        {
            _lifetimeCor = StartCoroutine(LifetimeRoutine(distance / speed));
            
            _damage = damage;
            _speed = speed;
            _attacker = attacker;
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
                var data = new GetDamageData(_damage, _attacker, Elementals.Fire);
                damageable.TakeDamage(data);
            }
            Break();
        }

        public void Break()
        {
            _pool.Despawn(this);
            
            if(_lifetimeCor != null)
                StopCoroutine(_lifetimeCor);
        }
    }
}