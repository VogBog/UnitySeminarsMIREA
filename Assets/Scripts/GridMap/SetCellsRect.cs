using UnityEngine;

namespace GridMap
{
    public readonly struct SetCellsRect
    {
        public readonly RectInt Rect;
        public readonly byte Value;

        public SetCellsRect(RectInt rect, byte value)
        {
            Rect = rect;
            Value = value;
        }
    }
}