using System;
using UnityEngine;

namespace Game.Player
{
    public interface ISnakeTail
    {
        event Action<SnakePoint> AddNewPoint; 
        
        void SetPrefab(SnakePoint prefab);
        void Instantiate(Vector3 position, Quaternion rotation);
    }
}