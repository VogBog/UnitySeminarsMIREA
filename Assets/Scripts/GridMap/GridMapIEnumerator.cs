using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridMap
{
    public struct GridMapIEnumerator : IEnumerator<(int, int, byte)>, IEnumerable<(int, int, byte)>
    {
        private readonly Rect _rect;
        private readonly byte[,] _map;
        private int _x;
        private int _y;
        
        public (int, int, byte) Current { get; private set; }
        object IEnumerator.Current => Current;

        public GridMapIEnumerator(Rect rect, byte[,] map)
        {
            _rect = rect;
            _map = map;
            _x = (int)rect.xMin;
            _y = (int)rect.yMin;
            Current = default;
        }
        
        public bool MoveNext()
        {
            if (_x < _rect.xMax)
            {
                Current = (++_x, _y, _map[_x, _y]);
                return true;
            }

            _x = (int)_rect.xMin;

            if (_y < _rect.yMax)
            {
                Current = (_x, ++_y, _map[_x, _y]);
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _x = (int)_rect.xMin;
            _y = (int)_rect.yMin;
            Current = default;
        }

        public void Dispose()
        {
            
        }

        public IEnumerator<(int, int, byte)> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}