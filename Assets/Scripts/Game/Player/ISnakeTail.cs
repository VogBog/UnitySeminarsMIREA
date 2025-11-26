using System;
using MultiNetwork;
using UnityEngine;

namespace Game.Player
{
    public interface ISnakeTail : INetworkTypeRequirer
    {
        event Action<SnakePoint> AddNewPoint;
        
        void SetSnake(SnakeTail tail);
        
        void SetPrefab(SnakePoint prefab);
        void Instantiate(Vector3 position, Quaternion rotation, Action<SnakePoint> onSpawn);
        void Despawn(SnakePoint point);

        void CalculateDataForAddLength(Action<SnakeTailCalculationData> onCalculationData);
    }
}