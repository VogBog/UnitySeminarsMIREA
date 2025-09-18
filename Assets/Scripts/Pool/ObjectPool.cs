using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class ObjectPool : MonoBehaviour
    {
        private readonly Dictionary<Type, PooledPrefab> _prefabs = new();
        private readonly Dictionary<Type, Stack<Component>> _pool = new();
        private readonly Dictionary<Type, List<Component>> _spawnedObjects = new();

        public void RegisterPrefab(Type type, PooledPrefab prefab)
        {
            _prefabs.TryAdd(type, prefab);
        }

        public void CreateInstances(Type type, int count)
        {
            if (!_prefabs.TryGetValue(type, out var prefab) || count < 1)
                return;
            
            _pool.TryAdd(type, new Stack<Component>());
            var stack = _pool[type];

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(prefab.Prefab);
                instance.gameObject.SetActive(false);
                prefab.InstantiatedCallback?.Invoke(this, instance);
                
                stack.Push(instance);
            }
        }

        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count)
        {
            RegisterPrefab(type, prefab);
            CreateInstances(type, count);
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            var result = Spawn(position, rotation, typeof(T));
            if (result is not T component)
                throw new ArgumentException($"ObjectPool::Spawn: result type is not {typeof(T).Name}");
            return component;
        }

        public Component Spawn(Vector3 position, Quaternion rotation, Type type)
        {
            if(!_prefabs.TryGetValue(type, out var prefab))
                throw new NullReferenceException($"ObjectPool::Spawn: type {type.Name} does not registered");
            
            if (!_pool.TryGetValue(type, out var stack))
            {
                CreateInstances(type, 4);
                stack = _pool[type];
            }
            
            if(stack.Count == 0)
                CreateInstances(type, 4);
            
            var component = stack.Pop();
            
            _spawnedObjects.TryAdd(type, new List<Component>());
            _spawnedObjects[type].Add(component);
            
            component.gameObject.SetActive(true);
            component.transform.SetPositionAndRotation(position, rotation);
            prefab.SpawnedCallback?.Invoke(this, component);
            
            return component;
        }
        
        public void Despawn<T>(T component) where T : Component => Despawn(component, typeof(T));

        public void Despawn(Component component, Type type)
        {
            if(!_spawnedObjects.TryGetValue(type, out var list))
                throw new NullReferenceException($"ObjectPool::Despawn: Hasn't registered anything of type {type.Name}");
            int index = list.IndexOf(component);
            if (index == -1)
                return;
            
            list.RemoveAt(index);
            _pool[type].Push(component);
        }
    }
}