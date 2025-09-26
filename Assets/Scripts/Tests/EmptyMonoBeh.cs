using System;
using GridMap.Tiles;
using UnityEngine;

namespace Tests
{
    public class EmptyMonoBeh : MonoBehaviour, ITile
    {
        private void Awake()
        {
            enabled = false;
        }

        public event Action<Component, ITile> Hided;
        
        public virtual void OnShow()
        {
            
        }

        public virtual void OnHide()
        {
            Hided?.Invoke(this, this);
        }

        protected void InvokeHide()
        {
            Hided?.Invoke(this, this);
        }
    }
}