using System.Collections;
using Damage;
using Data;
using Extensions;
using GridMap;
using Pool;
using UnityEngine;

namespace Player.Abilities.Ice
{
    public class IceWall : MonoBehaviour, IDamageable
    {
        private ObjectPool _pool;
        private GridMap.GridMap _gridMap;
        private Coroutine _lifetimeCor;

        private int _health;
        private float _time;
        private bool _broke;
        
        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap.GridMap>();
        }
        
        public void SetWall(int health, float time)
        {
            _health = health;
            _time = time;
            _broke = false;
            
            var rects = _gridMap.FromWorldRectToIndexesRect(
                transform.position.x, transform.position.z,
                transform.localScale.z, transform.localScale.x,
                transform.forward, GridMapValues.IceFloor);
            
            _gridMap.SetCellsAsync(rects);

            StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_time);

            Break();
        }

        public void Break()
        {
            if(_lifetimeCor != null)
                StopCoroutine(_lifetimeCor);

            if (_broke)
                return;
            _broke = true;
            
            _pool.Despawn(this);
        }

        public void TakeDamage(ref GetDamageData data)
        {
            if (data.Damage <= 0)
                return;
            
            if(data.Elemental == Elementals.Fire)
                data.Damage *= 2;
            
            _health -= data.Damage;
            if (_health <= 0)
            {
                Break();
            }
        }
    }
}