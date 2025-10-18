using Damage;
using Extensions;
using GridMap;
using UnityEngine;

namespace SceneObjects
{
    [RequireComponent(typeof(Collider))]
    public class WaterBigBottle : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private int _initHealth;
        
        private GridMap.GridMap _gridMap;
        private int _health;
        
        private void Awake()
        {
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap.GridMap>();
            _health = _initHealth;
        }
        
        public void TakeDamage(ref GetDamageData data)
        {
            _health--;

            if (_health != 0)
                return;
            
            var cells = _gridMap.FromWorldSphereToIndexesSphere(
                transform.position.x, transform.position.z, _explosionRadius, GridMapValues.Water);
            _gridMap.SetCellsAsync(cells);
            
            Destroy(gameObject);
        }
    }
}