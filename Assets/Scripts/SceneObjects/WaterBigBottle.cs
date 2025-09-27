using Damage;
using Data;
using Extensions;
using GridMap;
using UnityEngine;

namespace SceneObjects
{
    [RequireComponent(typeof(Collider))]
    public class WaterBigBottle : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _explosionRadius;
        
        private GridMap.GridMap _gridMap;
        
        private void Awake()
        {
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap.GridMap>();
        }
        
        public void TakeDamage(GetDamageData data)
        {
            if (data.Elemental != Elementals.Ice)
                return;
            
            var cells = _gridMap.FromWorldSphereToIndexesSphere(
                transform.position.x, transform.position.z, _explosionRadius, GridMapValues.IceFloor);
            _gridMap.SetCellsAsync(cells);
            
            Destroy(gameObject);
        }
    }
}