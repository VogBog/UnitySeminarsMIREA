using System.Collections;
using Pool;
using UnityEngine;

namespace Player.Abilities
{
    public class ProjectileParticles : MonoBehaviour
    {
        private ObjectPool _pool;
        
        [SerializeField] private ParticleSystem _particleSystem;

        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
        }

        public void Connect(Transform parent)
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            _particleSystem.Play();
        }

        public void Stop()
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            StartCoroutine(StopRoutine());
        }

        private IEnumerator StopRoutine()
        {
            _particleSystem.Stop();

            yield return new WaitForSeconds(3f);
            
            _pool.Despawn(this, GetType());
        }
    }
}