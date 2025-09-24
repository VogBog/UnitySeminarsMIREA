using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridMap
{
    public struct GridMapIEnumerator : IEnumerator<(int, int, byte)>, IEnumerable<(int, int, byte)>
    {
        private readonly RectInt _rect;
        private readonly byte[,] _map;
        private int _x;
        private int _y;
        
        public (int, int, byte) Current { get; private set; }
        object IEnumerator.Current => Current;

        public GridMapIEnumerator(RectInt rect, byte[,] map)
        {
            _rect = rect;
            _map = map;
            _x = rect.xMin;
            _y = rect.yMin;
            Current = default;
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (_x < _rect.xMax)
                {
                    if (_x >= _map.GetLength(0) - 1)
                    {
                        _x++;
                        continue;
                    }
                    
                    if (_x < 0) _x = -1;

                    Current = (++_x, _y, _map[_x, _y]);

                    return true;
                }

                _x = _rect.xMin;

                if (_y < _rect.yMax)
                {
                    if (_y >= _map.GetLength(1) - 1)
                    {
                        _y++;
                        continue;
                    }
                    
                    if (_y < 0) _y = -1;

                    Current = (_x, ++_y, _map[_x, _y]);
                    return true;
                }

                return false;
            }
        }

        public void Reset()
        {
            _x = _rect.xMin;
            _y = _rect.yMin;
            Current = default;
        }

        public void Dispose()
        {
            
        }

        public IEnumerator<(int, int, byte)> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}