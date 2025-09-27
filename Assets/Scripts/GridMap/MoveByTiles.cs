using System;
using Extensions;
using UnityEngine;

namespace GridMap
{
    public class MoveByTiles : MonoBehaviour
    {
        private GridMap _gridMap;
        private byte _cellType = 0;
        private Vector2Int _indexes = new Vector2Int(0, 0);

        public event Action<byte, Vector2Int> WentToNewCell;
        public event Action<byte, Vector2Int> WentToNewCellType; 
        
        private void Awake()
        {
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap>();
        }
        
        private void FixedUpdate()
        {
            var (x, y) = _gridMap.FromWorldPositionToIndexes(transform.position.x, transform.position.z);
            byte type = _gridMap.GetCell(x, y);
            
            if (_indexes.x != x || _indexes.y != y || _cellType != type)
            {
                _indexes = new Vector2Int(x, y);
                WentToNewCell?.Invoke(type, _indexes);

                if (type != _cellType)
                {
                    _cellType = type;
                    WentToNewCellType?.Invoke(type, _indexes);
                }
            }
        }
    }
}