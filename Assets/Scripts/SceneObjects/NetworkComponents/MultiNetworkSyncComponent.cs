using System;
using System.Collections.Generic;
using Data;
using MainMenu;
using UnityEngine;

namespace SceneObjects.NetworkComponents
{
    public abstract class MultiNetworkSyncComponent : MonoBehaviour
    {
        protected abstract void GetRequiredComponents(IList<Type> components);
        protected abstract void DeactivateUselessComponents(NetworkTypes type, IDictionary<Type, Component> components);

        public void CheckAndAddMissingComponents(IList<Type> components)
        {
            foreach (var type in components)
            {
                if (!gameObject.TryGetComponent(type, out _))
                    gameObject.AddComponent(type);
            }
        }

        public IDictionary<Type, Component> CheckAndThrowMissingComponents(IList<Type> components)
        {
            var result = new Dictionary<Type, Component>();
            foreach (var type in components)
            {
                if (gameObject.TryGetComponent(type, out var component))
                {
                    result.Add(type, component);
                }
                else
                {
                    Debug.LogError($"Missing component {type} on object {gameObject.name}", gameObject);
                }
            }
            
            return result;
        }

        private void Awake()
        {
            var list = new List<Type>();
            GetRequiredComponents(list);
            DeactivateUselessComponents(StaticParameters.NetworkType, CheckAndThrowMissingComponents(list));
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            var list = new List<Type>();
            GetRequiredComponents(list);
            CheckAndAddMissingComponents(list);
        }
        #endif
    }
}