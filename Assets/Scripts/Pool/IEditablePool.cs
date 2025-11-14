using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public interface IEditablePool : IObjectPool
    {
        Dictionary<Type, PooledPrefab> GetPrefabs();
        Dictionary<Type, Stack<Component>> GetPoolObjects();
        Dictionary<Type, List<Component>> GetSpawnedObjects();
    }
}