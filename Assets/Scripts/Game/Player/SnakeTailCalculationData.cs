using UnityEngine;

namespace Game.Player
{
    public readonly struct SnakeTailCalculationData
    {
        public readonly Vector3 LastPoint;

        public SnakeTailCalculationData(Vector3 lastPoint)
        {
            LastPoint = lastPoint;
        }
    }
}