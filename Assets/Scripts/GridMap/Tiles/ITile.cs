using System;
using UnityEngine;

namespace GridMap.Tiles
{
    public interface ITile
    {
        event Action<Component, ITile> Hided; 
        
        void OnShow();
        void OnHide();
    }
}