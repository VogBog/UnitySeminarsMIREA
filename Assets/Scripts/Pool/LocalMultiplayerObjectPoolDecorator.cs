using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class LocalMultiplayerObjectPoolDecorator : IObjectPool, IEditablePool
    {
        private readonly IObjectPool _pool;
        private readonly IEditablePool _editablePool;
        
        public LocalMultiplayerObjectPoolDecorator(IObjectPool pool, LocalMultiplayerObjectPoolNetworkObject lmPool)
        {
            var comp = lmPool;
            _pool = comp;
            comp.SetPool(pool);
            
            if(pool is IEditablePool editablePool)
                _editablePool = editablePool;
        }

        public void SetInstantiator(IObjectPoolInstantiator instantiator) => _pool.SetInstantiator(instantiator);

        public void RegisterPrefab(Type type, PooledPrefab prefab) => _pool.RegisterPrefab(type, prefab);

        public void CreateInstances(Type type, int count) => _pool.CreateInstances(type, count);

        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count)
            => _pool.RegisterAndInstantiatePrefab(type, prefab, count);

        public void Spawn<T>(Vector3 position, Quaternion rotation, Action<T> onSpawn) where T : Component
            => _pool.Spawn<T>(position, rotation, onSpawn);

        public void Spawn(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn)
            => _pool.Spawn(position, rotation, type, onSpawn);

        public void Despawn<T>(T component) where T : Component
            => _pool.Despawn(component);

        public void Despawn(Component component, Type type)
            => _pool.Despawn(component, type);

        public Dictionary<Type, Stack<Component>> GetPoolObjects() => _editablePool.GetPoolObjects();

        public Dictionary<Type, List<Component>> GetSpawnedObjects() => _editablePool.GetSpawnedObjects();
    }
}