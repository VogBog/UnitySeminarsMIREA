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
        }
        
        public void CreateGrid()
        {
            throw new NotImplementedException();
        }

        public byte GetCell(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetCell(int x, int y, byte c)
        {
            throw new NotImplementedException();
        }

        public void SetCellsAsync(SetCellsRect rect)
        {
            throw new NotImplementedException();
        }

        public void SetCellsAsync(IEnumerable<SetCellsRect> rects)
        {
            throw new NotImplementedException();
        }

        public GridMapIEnumerator GetAreaIEnumerator(RectInt rect)
        {
            throw new NotImplementedException();
        }

        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk)
        {
            throw new NotImplementedException();
        }

        public (float, float) FromIndexesToWorldPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public (int, int) FromWorldPositionToIndexes(float x, float y)
        {
            throw new NotImplementedException();
        }

        public SetCellsRect FromWorldRangeToIndexesRange(float startX, float startY, float endX, float endY, byte value)
        {
            throw new NotImplementedException();
        }

        public List<SetCellsRect> FromWorldSphereToIndexesSphere(float posX, float posY, float radius, byte value)
        {
            throw new NotImplementedException();
        }

        public List<SetCellsRect> FromWorldRectToIndexesRect(float posX, float posY, float width, float height, Vector3 direction, byte value)
        {
            throw new NotImplementedException();
        }

        public List<SetCellsRect> RasterizeRect(Vector2 lb, Vector2 lt, Vector2 rb, Vector2 rt, byte value)
        {
            throw new NotImplementedException();
        }

        public List<SetCellsRect> PaintConnectedAreaByPredicate(int centerX, int centerY, int maxDist, byte value, Predicate<byte> predicate)
        {
            throw new NotImplementedException();
        }

        public void DelayedCall(Action call)
        {
            throw new NotImplementedException();
        }
    }
}