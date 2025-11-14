using System;
using UnityEngine;

namespace Pool
{
    public class DefaultObjectPoolInstantiator : IObjectPoolInstantiator
    {
        public void InstantiateObject(Type type, PooledPrefab prefab, Transform parent, Action<Component> onInstantiate)
        {
            var result = UnityEngine.Object.Instantiate(prefab.Prefab, parent);
            onInstantiate?.Invoke(result);
        }
    }
}