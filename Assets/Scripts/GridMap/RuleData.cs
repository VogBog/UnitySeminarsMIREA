using UnityEngine;

namespace GridMap
{
    public readonly struct RuleData
    {
        public readonly GridMap Map;
        public readonly Vector2Int Indexes;
        public readonly byte From;
        public readonly byte To;

        public RuleData(GridMap map, Vector2Int indexes, byte from, byte to)
        {
            Map = map;
            Indexes = indexes;
            From = from;
            To = to;
        }
    }
}