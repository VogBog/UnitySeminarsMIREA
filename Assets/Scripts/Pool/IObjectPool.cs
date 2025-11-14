using System;
using UnityEngine;

namespace Pool
{
    public interface IObjectPool
    {
        void SetInstantiator(IObjectPoolInstantiator instantiator);
        public void RegisterPrefab(Type type, PooledPrefab prefab);
        public void CreateInstances(Type type, int count);
        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count);
        public void Spawn<T>(Vector3 position, Quaternion rotation, Action<T> onSpawn) where T : Component;
        public void Spawn(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn);
        public void Despawn<T>(T component) where T : Component;
        public void Despawn(Component component, Type type);
    }
}