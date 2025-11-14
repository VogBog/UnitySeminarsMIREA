using System;
using UnityEngine;

namespace Pool
{
    public class ObjectPool : MonoBehaviour
    {
        private IObjectPool _pool;

        public void SetPool(IObjectPool pool)
        {
            _pool = pool;
        }
        
        public IObjectPool GetPool() => _pool;

        public void RegisterPrefab(Type type, PooledPrefab prefab) => _pool.RegisterPrefab(type, prefab);

        public void CreateInstances(Type type, int count) => _pool.CreateInstances(type, count);

        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count)
            => _pool.RegisterAndInstantiatePrefab(type, prefab, count);

        public void Spawn<T>(Vector3 position, Quaternion rotation, Action<T> onSpawn) where T : Component
            => _pool.Spawn<T>(position, rotation, onSpawn);

        public void Spawn(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn)
            => _pool.Spawn(position, rotation, type, onSpawn);
        
        public void Despawn<T>(T component) where T : Component => _pool.Despawn(component, typeof(T));

        public void Despawn(Component component, Type type) => _pool.Despawn(component, type);
    }
}