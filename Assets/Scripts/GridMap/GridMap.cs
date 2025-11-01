using System;
using System.Collections.Generic;
using GridMap.NetworkPolitics;
using UnityEngine;

namespace GridMap
{
    public class GridMap : MonoBehaviour, IGridMap
    {
        [SerializeField] private Vector2 _bounds;
        [SerializeField] private float _cellsPerUnit;
        [SerializeField] private int _chunkSize;

        private IGridMap _map;

        public Vector2 Bounds => _bounds;
        public float CellsPerUnit => _cellsPerUnit;
        public int ChunkSize => _chunkSize;
        
        public event Action<Vector2Int> ChunkChanged;

        public void Initialize(IGridMap map)
        {
            _map = map;
            map.ChunkChanged += i => ChunkChanged?.Invoke(i);
        }

        public void CreateGrid() => _map.CreateGrid();

        public byte GetCell(int x, int y) => _map.GetCell(x, y);

        public void SetCell(int x, int y, byte c) => _map.SetCell(x, y, c);

        public void SetCellsAsync(SetCellsRect rect) => _map.SetCellsAsync(rect);

        public void SetCellsAsync(IEnumerable<SetCellsRect> rects) => _map.SetCellsAsync(rects);

        public GridMapIEnumerator GetAreaIEnumerator(RectInt rect) => _map.GetAreaIEnumerator(rect);

        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk) => _map.GetChunkIEnumerator(chunk);

        public (float, float) FromIndexesToWorldPosition(int x, int y) => _map.FromIndexesToWorldPosition(x, y);

        public (int, int) FromWorldPositionToIndexes(float x, float y) => _map.FromWorldPositionToIndexes(x, y);

        public SetCellsRect FromWorldRangeToIndexesRange(float startX, float startY, float endX, float endY, byte value)
            => _map.FromWorldRangeToIndexesRange(startX, startY, endX, endY, value);

        public List<SetCellsRect> FromWorldSphereToIndexesSphere(float posX, float posY, float radius, byte value)
            => _map.FromWorldSphereToIndexesSphere(posX, posY, radius, value);

        public List<SetCellsRect> FromWorldRectToIndexesRect(
            float posX, float posY, float width, float height, Vector3 direction, byte value)
        => _map.FromWorldRectToIndexesRect(posX, posY, width, height, direction, value);

        public List<SetCellsRect> RasterizeRect(Vector2 lb, Vector2 lt, Vector2 rb, Vector2 rt, byte value)
            => _map.RasterizeRect(lb, rt, rb, rt, value);

        public List<SetCellsRect> PaintConnectedAreaByPredicate(
            int centerX, int centerY, int maxDist, byte value, Predicate<byte> predicate)
        => _map.PaintConnectedAreaByPredicate(centerX, centerY, maxDist, value, predicate);

        public void DelayedCall(Action call) => _map.DelayedCall(call);
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_cellsPerUnit == 0)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_bounds.x, 40f, _bounds.y));
            Gizmos.DrawWireCube(Vector3.zero,
                new Vector3(_chunkSize / _cellsPerUnit, _chunkSize / _cellsPerUnit, _chunkSize / _cellsPerUnit));
        }
#endif
    }
}