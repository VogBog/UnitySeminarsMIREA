using UnityEngine;

namespace GridMap
{
    public readonly struct SetCellsRect
    {
        public readonly Rect Rect;
        public readonly byte Value;

        public SetCellsRect(Rect rect, byte value)
        {
            Rect = rect;
            Value = value;
        }
    }
}