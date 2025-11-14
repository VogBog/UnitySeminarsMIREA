using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace MultiNetwork
{
    public abstract class MultiNetworkComponent : MonoBehaviour
    {
        protected abstract void RequireComponents(List<Type> components);
        protected abstract void DestroyRedundantComponents(StaticParameters.NetworkTypes type);

        private List<Type> ForEachIfMissing(Action<Type> action)
        {
            var list = new List<Type>();
            RequireComponents(list);
            foreach (var type in list)
            {
                if(!gameObject.TryGetComponent(type, out _))
                    action.Invoke(type);
            }

            return list;
        }

        private List<Type> ThrowIfMissing()
        {
            return ForEachIfMissing(type => Debug.LogError($"Missing component: {type}", gameObject));
        }

        private List<Type> AddMissing()
        {
            return ForEachIfMissing(type => gameObject.AddComponent(type));
        }

        private void Awake()
        {
            ThrowIfMissing();
            DestroyRedundantComponents(StaticParameters.NetworkType);
            
            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            AddMissing();
        }
#endif
    }
}