using System;
using System.Collections.Generic;
using Global;
using MainMenu;
using UnityEngine;

namespace Network.General
{
    public abstract class AbstractNetwork : MonoBehaviour
    {
        protected virtual bool DestroyAfterAwake => true;
        
        private void Awake()
        {
            CheckRequireComponents();
            BeforeAwake();
            
            if(StaticParameters.GameType is not GameTypes.LocalMultiplayer)
                IsNotLocalMultiplayer();
            
            if(DestroyAfterAwake)
                Destroy(this);
        }

        private void CheckRequireComponents()
        {
            var list = new List<Type>();
            AddRequireComponents(list);
            
            foreach (var type in list)
            {
                var component = GetComponent(type);
                if (component is null)
                {
                    gameObject.AddComponent(type);
                }
            }
        }

        protected virtual void BeforeAwake()
        {
        }

        protected abstract void AddRequireComponents(List<Type> components);

        protected abstract void IsNotLocalMultiplayer();

        protected virtual void OnValidate()
        {
            CheckRequireComponents();
        }
    }
}