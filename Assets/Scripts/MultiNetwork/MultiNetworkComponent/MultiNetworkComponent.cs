using System;
using System.Collections.Generic;
using Global;
using UnityEngine;

namespace MultiNetwork.MultiNetworkComponent
{
    public abstract class MultiNetworkComponent : MonoBehaviour
    {
        protected abstract void RequireComponents(List<Type> components);
        protected abstract void RemoveRedundantComponents(
            StaticParameters.NetworkTypes type, Dictionary<Type, Component> components);

        private void Awake()
        {
            var types = ThrowMissingComponents();
            var dict = new Dictionary<Type, Component>();

            foreach (var type in types)
            {
                dict.Add(type, GetComponent(type));
            }
            
            RemoveRedundantComponents(StaticParameters.NetworkType, dict);
            
            Destroy(this);
        }

        private List<Type> CheckAllComponents(Action<Type> forEach)
        {
            var list = new List<Type>();
            RequireComponents(list);

            foreach (var type in list)
            {
                forEach.Invoke(type);
            }

            return list;
        }

        private List<Type> AddMissingComponents()
        {
            return CheckAllComponents(type =>
            {
                if(!gameObject.TryGetComponent(type, out _))
                    gameObject.AddComponent(type);
            });
        }

        private List<Type> ThrowMissingComponents()
        {
            return CheckAllComponents(type =>
            {
                if(!gameObject.TryGetComponent(type, out _))
                    Debug.LogError($"Object {gameObject.name} has missing component {type.Name}", gameObject);
            });
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            AddMissingComponents();
        }
        #endif
    }
}