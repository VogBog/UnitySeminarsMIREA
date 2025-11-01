using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridMap.NetworkPolitics
{
    public interface IGridMap
    {
        public Vector2 Bounds { get; }
        public float CellsPerUnit { get; }
        public int ChunkSize { get; }

        public event Action<Vector2Int> ChunkChanged;

        public void CreateGrid();
        public byte GetCell(int x, int y);
        public void SetCell(int x, int y, byte c);
        public void SetCellsAsync(SetCellsRect rect);
        public void SetCellsAsync(IEnumerable<SetCellsRect> rects);
        public GridMapIEnumerator GetAreaIEnumerator(RectInt rect);
        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk);
        public (float, float) FromIndexesToWorldPosition(int x, int y);
        public (int, int) FromWorldPositionToIndexes(float x, float y);
        public SetCellsRect
            FromWorldRangeToIndexesRange(float startX, float startY, float endX, float endY, byte value);
        public List<SetCellsRect> FromWorldSphereToIndexesSphere(float posX, float posY, float radius, byte value);
        public List<SetCellsRect> FromWorldRectToIndexesRect(float posX, float posY, float width, float height,
            Vector3 direction, byte value);
        public List<SetCellsRect> RasterizeRect(Vector2 lb, Vector2 lt, Vector2 rb, Vector2 rt, byte value);
        
        public static Rect CalculateAABB(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float minX = Mathf.Min(a.x, b.x, c.x, d.x);
            float minY = Mathf.Min(a.y, b.y, c.y, d.y);
            float maxX = Mathf.Max(a.x, b.x, c.x, d.x);
            float maxY = Mathf.Max(a.y, b.y, c.y, d.y);
            
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        public static RectInt CalculateAABBInt(IEnumerable<Vector2Int> points)
        {
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            int minX = int.MaxValue;
            int maxX = int.MinValue;

            foreach (var i in points)
            {
                if(i.x < minX) minX = i.x;
                if(i.x > maxX) maxX = i.x;
                if(i.y < minY) minY = i.y;
                if(i.y > maxY) maxY = i.y;
            }
            
            return new(minX, minY, maxX - minX, maxY - minY);
        }

        public List<SetCellsRect> PaintConnectedAreaByPredicate(
            int centerX, int centerY, int maxDist, byte value, Predicate<byte> predicate);

        public void DelayedCall(Action call);
    }
}