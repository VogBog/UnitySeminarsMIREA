using System;
using UnityEngine;

namespace Pool
{
    public interface IObjectPoolInstantiator
    {
        void InstantiateObject(Type type, PooledPrefab prefab, Transform parent, Action<Component> onInstantiate);
    }
}