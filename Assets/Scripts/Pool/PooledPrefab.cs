using System;
using UnityEngine;

namespace Pool
{
    public readonly struct PooledPrefab
    {
        public readonly Component Prefab;
        public readonly Action<ObjectPool, Component> InstantiatedCallback;
        public readonly Action<ObjectPool, Component> SpawnedCallback;

        public PooledPrefab(
            Component prefab,
            Action<ObjectPool, Component> onInstantiating,
            Action<ObjectPool, Component> onSpawn)
        {
            Prefab = prefab;
            InstantiatedCallback = onInstantiating;
            SpawnedCallback = onSpawn;
        }

        public PooledPrefab(Component prefab)
        {
            Prefab = prefab;
            InstantiatedCallback = null;
            SpawnedCallback = null;
        }
    }
}