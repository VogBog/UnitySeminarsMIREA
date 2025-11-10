using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GridMap.NetworkPolitics
{
    public class LocalMultiplayerGridMap : NetworkBehaviour, IGridMap
    {
        private IGridMap _map;
        
        public Vector2 Bounds => _map.Bounds;
        public float CellsPerUnit => _map.CellsPerUnit;
        public int ChunkSize => _map.ChunkSize;
        
        public event Action<Vector2Int> ChunkChanged;

        public void Initialize(IGridMap gridMap)
        {
            _map = gridMap;
            _map.ChunkChanged += ev => ChunkChanged?.Invoke(ev);
        }

        public void CreateGrid() => _map.CreateGrid();

        public byte GetCell(int x, int y) => _map.GetCell(x, y);

        public void SetCell(int x, int y, byte c)
        {
            SetCellRpc(x, y, c);
        }

        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        private void SetCellRpc(int x, int y, byte c) => _map.SetCell(x, y, c);

        public void SetCellsAsync(SetCellsRect rect)
        {
            var (a, b, c, d, e) = rect.ToRawData();
            SetCellsAsyncRpc(a, b, c, d, e);
        }

        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        private void SetCellsAsyncRpc(int xMin, int yMin, int width, int height, byte value)
        {
            var rect = SetCellsRect.FromRawData(xMin, yMin, width, height, value);
            _map.SetCellsAsync(rect);
        }

        public void SetCellsAsync(IEnumerable<SetCellsRect> rects)
        {
            var (a, b, c, d, e) = SetCellsRect.ToRawDataArray(rects);
            SetCellsAsyncRpc(a, b, c, d, e);
        }

        [Rpc(SendTo.Everyone, RequireOwnership = false)]
        private void SetCellsAsyncRpc(int[] x, int[] y, int[] widths, int[] heights, byte[] values)
        {
            var array = SetCellsRect.FromRawDataArray(x, y, widths, heights, values);
            _map.SetCellsAsync(array);
        }

        public GridMapIEnumerator GetAreaIEnumerator(RectInt rect) => _map.GetAreaIEnumerator(rect);

        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk) => _map.GetChunkIEnumerator(chunk);

        public (float, float) FromIndexesToWorldPosition(int x, int y) => _map.FromIndexesToWorldPosition(x, y);

        public (int, int) FromWorldPositionToIndexes(float x, float y) => _map.FromWorldPositionToIndexes(x, y);

        public SetCellsRect FromWorldRangeToIndexesRange(float startX, float startY, float endX, float endY, byte value)
        => _map.FromWorldRangeToIndexesRange(startX, startY, endX, endY, value);

        public List<SetCellsRect> FromWorldSphereToIndexesSphere(float posX, float posY, float radius, byte value)
        => _map.FromWorldSphereToIndexesSphere(posX, posY, radius, value);

        public List<SetCellsRect> FromWorldRectToIndexesRect(float posX, float posY, float width, float height, Vector3 direction, byte value)
        => _map.FromWorldRectToIndexesRect(posX, posY, width, height, direction, value);

        public List<SetCellsRect> RasterizeRect(Vector2 lb, Vector2 lt, Vector2 rb, Vector2 rt, byte value)
            => _map.RasterizeRect(lb, lt, rb, rt, value);

        public List<SetCellsRect> PaintConnectedAreaByPredicate(int centerX, int centerY, int maxDist, byte value,
            Predicate<byte> predicate)
            => _map.PaintConnectedAreaByPredicate(centerX, centerY, maxDist, value, predicate);

        public void DelayedCall(Action call) => _map.DelayedCall(call);
    }
}