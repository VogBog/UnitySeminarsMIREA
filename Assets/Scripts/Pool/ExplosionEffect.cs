using System.Collections;
using UnityEngine;

namespace Pool
{
    public class ExplosionEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        private ObjectPool _pool;
        
        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
        }

        public void Explode()
        {
            StartCoroutine(ExplodeRoutine());
        }

        private IEnumerator ExplodeRoutine()
        {
            _particleSystem.Play();

            yield return new WaitForSeconds(2f);
            
            _particleSystem.Stop();

            yield return new WaitForSeconds(0.5f);
            
            _pool.Despawn(this, GetType());
        }
    }
}