using System;
using UnityEngine;

namespace Pool
{
    public interface IObjectPool
    {
        public void RegisterPrefab(Type type, PooledPrefab prefab);
        public void CreateInstances(Type type, int count);
        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count);
        public T Spawn<T>(Vector3 position, Quaternion rotation) where T : Component;
        public Component Spawn(Vector3 position, Quaternion rotation, Type type);
        public void Despawn<T>(T component) where T : Component;
        public void Despawn(Component component, Type type);
    }
}