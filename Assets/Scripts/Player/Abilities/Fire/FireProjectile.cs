using System.Collections;
using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    public class FireProjectile : MonoBehaviour
    {
        private float _speed;
        private float _damage;
        private Coroutine _lifetimeCor;
        private ObjectPool _pool;

        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
        }
        
        public void Throw(float speed, float distance, float damage)
        {
            _lifetimeCor = StartCoroutine(LifetimeRoutine(distance / speed));
            
            _damage = damage;
            _speed = speed;
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