using System;
using System.Collections;
using System.Collections.Generic;
using Tests;
using UnityEngine;

namespace GridMap
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField] private Vector2 _bounds;
        [SerializeField] private float _cellsPerUnit;
        [SerializeField] private int _chunkSize;

        private byte[,] _map;
        private readonly Queue<SetCellsRect> _jobs = new();
        private bool _processingJobs;
        private readonly List<Vector2Int> _cellsBuffer = new();
        private Vector3 _border;
        
        public Vector2 Bounds => _bounds;
        public float CellsPerUnit => _cellsPerUnit;
        public int ChunkSize => _chunkSize;

        public event Action<Vector2Int> ChunkChanged;

        private void Awake()
        {
            CreateGrid();
            StartCoroutine(LifetimeRoutine());
        }

        private IEnumerator LifetimeRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);

                int maxChunkX = _map.GetLength(0) / _chunkSize;
                int maxChunkY = _map.GetLength(1) / _chunkSize;

                for (int chunkX = 0; chunkX < maxChunkX; chunkX++)
                {
                    for (int chunkY = 0; chunkY < maxChunkY; chunkY++)
                    {
                        var chunk = new Vector2Int(chunkX, chunkY);

                        int minX = chunkX * _chunkSize;
                        int minY = chunkY * _chunkSize;

                        int maxX = minX + _chunkSize;
                        int maxY = minY + _chunkSize;

                        bool changed = false;

                        for (int x = minX; x < maxX; x++)
                        {
                            for (int y = minY; y < maxY; y++)
                            {
                                _map[x, y] = (byte)CellEvery5SecondsRule(_map[x, y], out bool isChanged);
                                changed |= isChanged;
                            }
                        }

                        if (changed)
                        {
                            ChunkChanged?.Invoke(chunk);
                        }

                        yield return null;
                    }
                }
            }
        }

        private int CellEvery5SecondsRule(byte value, out bool changed)
        {
            changed = false;
            
            if (value == 0) return 0;
            if (value == 1)
            {
                changed = true;
                return 0;
            }
            
            if (value <= GridMapValues.Fire25Seconds)
                return value - 1;

            changed = true;
            return 0;
        }

        private int SetCellRule(byte fromValue, byte toValue, out bool changed)
        {
            int result = GridMapRules.SetCellRule(fromValue, toValue);
            changed = fromValue != result;
            return result;
        }

        public void CreateGrid()
        {
            int width = Mathf.CeilToInt(_bounds.x * _cellsPerUnit);
            int height = Mathf.CeilToInt(_bounds.y * _cellsPerUnit);
            _map = new byte[width, height];
            _border = new Vector3(-_bounds.x / 2f, 0f, -_bounds.y / 2f);
        }
        
        public byte GetCell(int x, int y) => _map[x, y];

        public void SetCell(int x, int y, byte c)
        {
            int value = SetCellRule(_map[x, y], c, out bool changed);
            if (changed)
            {
                _map[x, y] = (byte)value;
                ChunkChanged?.Invoke(new(x / _chunkSize, y / _chunkSize));
            }
        }

        private void SetCells(SetCellsRect rect)
        {
            int minX = rect.Rect.xMin;
            int minY = rect.Rect.yMin;
            int maxX = rect.Rect.xMax;
            int maxY = rect.Rect.yMax;
            _cellsBuffer.Clear();

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    if (x < 0 || y < 0 || x >= _map.GetLength(0) || y >= _map.GetLength(1))
                    {
                        continue;
                    }

                    int value = SetCellRule(_map[x, y], rect.Value, out bool changed);
                    
                    if(!changed)
                        continue;
                    
                    _map[x, y] = (byte)value;
                    var chunk = new Vector2Int(x / _chunkSize, y / _chunkSize);
                    
                    if(!_cellsBuffer.Contains(chunk))
                        _cellsBuffer.Add(chunk);
                }
            }

            foreach (var chunk in _cellsBuffer)
            {
                ChunkChanged?.Invoke(chunk);
            }
        }

        public void SetCellsAsync(SetCellsRect rect)
        {
            if (rect.Rect.width < _chunkSize && rect.Rect.height < _chunkSize)
            {
                _jobs.Enqueue(rect);
                
                if (!_processingJobs)
                {
                    _processingJobs = true;
                    StartCoroutine(ProcessJobs());
                }
                
                return;
            }

            for (int x = rect.Rect.xMin; x <= rect.Rect.xMax; x += _chunkSize)
            {
                for (int y = rect.Rect.yMin; y <= rect.Rect.yMax; y += _chunkSize)
                {
                    int maxX = Mathf.Min(x + _chunkSize, rect.Rect.xMax);
                    int maxY = Mathf.Min(y + _chunkSize, rect.Rect.yMax);
                    var newRect = new RectInt(x, y, maxX - x, maxY - y);
                        
                    _jobs.Enqueue(new(newRect, rect.Value));
                }
            }
            
            if (!_processingJobs)
            {
                _processingJobs = true;
                StartCoroutine(ProcessJobs());
            }
        }

        public void SetCellsAsync(IEnumerable<SetCellsRect> rects)
        {
            foreach (var rect in rects)
            {
                SetCellsAsync(rect);
            }
        }

        private IEnumerator ProcessJobs()
        {
            _processingJobs = true;

            while (_jobs.Count > 0)
            {
                yield return new WaitForFixedUpdate();
                
                var job = _jobs.Dequeue();
                SetCells(job);
            }

            _processingJobs = false;
        }

        public GridMapIEnumerator GetAreaIEnumerator(RectInt rect)
            => new GridMapIEnumerator(rect, _map);

        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk)
        {
            int minX = chunk.x * _chunkSize;
            int minY = chunk.y * _chunkSize;

            var rect = new RectInt(minX, minY, _chunkSize - 1, _chunkSize - 1);
            return new GridMapIEnumerator(rect, _map);
        }

        public (float, float) FromIndexesToWorldPosition(int x, int y)
        {
            float worldX = _border.x + (x + .5f) / _cellsPerUnit;
            float worldY = _border.z + (y + .5f) / _cellsPerUnit;
            
            return (worldX, worldY);
        }

        public (int, int) FromWorldPositionToIndexes(float x, float y)
        {
            int xIndex = Mathf.FloorToInt((x - _border.x) * _cellsPerUnit);
            int yIndex = Mathf.FloorToInt((y - _border.z) * _cellsPerUnit);
            
            return (xIndex, yIndex);
        }

        public SetCellsRect FromWorldRangeToIndexesRange(float startX, float startY, float endX, float endY, byte value)
        {
            var (sX, sY) = FromWorldPositionToIndexes(startX, startY);
            var (eX, eY) = FromWorldPositionToIndexes(endX, endY);
            var rect = new RectInt(sX, sY, eX - sX, eY - sY);
            
            return new SetCellsRect(rect, value);
        }

        public List<SetCellsRect> FromWorldSphereToIndexesSphere(float posX, float posY, float radius, byte value)
        {
            var list = new List<SetCellsRect>();
            
            for (float y = posY + radius; y >= posY - radius; y -= 1f / _cellsPerUnit)
            {
                float dy = y - posY;
                float dx = Mathf.Sqrt(radius * radius - dy * dy);
                float leftX = posX - dx;
                float rightX = posX + dx;

                var (minX, maxX) = FromWorldPositionToIndexes(leftX, rightX);
                var (curY, _) = FromWorldPositionToIndexes(y, 0);

                var rect = new RectInt(minX, curY, maxX - minX, 1);
                list.Add(new(rect, value));
            }

            return list;
        }

        public List<SetCellsRect> FromWorldRectToIndexesRect(float posX, float posY, float width, float height,
            Vector3 direction, byte value)
        {
            EasyFiguresDrawer.DrawCube(
                new Vector3(posX, 1f, posY), new Vector3(height, 2f, width),
                Quaternion.LookRotation(direction, Vector3.up), Color.green, 5f);
            
            if (direction.x == 0)
            {
                var rect = FromWorldRangeToIndexesRange(
                    posX - height / 2f,
                    posY - width / 2f,
                    posX + height / 2f,
                    posY + width / 2f,
                    value);
                return new() { rect };
            }

            if (direction.z == 0)
            {
                var rect = FromWorldRangeToIndexesRange(
                    posX - width / 2f,
                    posY - height / 2f,
                    posX + width / 2f,
                    posY + height / 2f,
                    value);
                return new() { rect };
            }

            Vector2 dir2D = new Vector2(direction.x, direction.z);
            Vector2 per2D = Vector2.Perpendicular(dir2D);
            Vector2 center2D = new Vector2(posX, posY);

            width /= 2f;
            height /= 2f;

            Vector2 lb = center2D - dir2D * width - per2D * height;
            Vector2 lt = center2D - dir2D * width + per2D * height;
            Vector2 rb = center2D + dir2D * width - per2D * height;
            Vector2 rt = center2D + dir2D * width + per2D * height;

            return RasterizeRect(lb, lt, rb, rt, value);
        }

        public List<SetCellsRect> RasterizeRect(Vector2 lb, Vector2 lt, Vector2 rb, Vector2 rt, byte value)
        {
            var list = new List<SetCellsRect>();
            var aabb = CalculateAABB(lb, lt, rb, rt);
            var lines = new Vector4[]
            {
                new(lb.x, lb.y, lt.x, lt.y), new(lb.x, lb.y, rb.x, rb.y),
                new(rt.x, rt.y, lt.x, lt.y), new(rt.x, rt.y, rb.x, rb.y)
            };

            var buffer = new List<float>();
            
            for (float x = aabb.xMin + .5f / _cellsPerUnit; x < aabb.xMax; x += 1f / _cellsPerUnit)
            {
                buffer.Clear();
                
                foreach (var line in lines)
                {
                    if((line.x <= x || line.z >= x) &&
                       (line.z <= x || line.x >= x))
                        continue;

                    float t = (x - line.x) / (line.z - line.x);
                    Debug.Log(t);
                    float yIntersection = line.y + t * (line.w - line.y);
                    buffer.Add(yIntersection);
                }
                
                if(buffer.Count < 2)
                    continue;
                if(buffer.Count > 2)
                    buffer.Sort();
                
                float y1 = buffer[0];
                float y2 = buffer[^1];
                
                if(Mathf.Abs(y1 - y2) < .5f / _cellsPerUnit)
                    continue;

                EasyFiguresDrawer.DrawCube(
                    new Vector3(
                        x,
                        1f,
                        (y1 + y2) / 2f),
                    new Vector3(
                        1f / _cellsPerUnit,
                        2f,
                        Mathf.Abs(y1 - y2)),
                    Quaternion.identity,
                    Color.red,
                    5f);
                
                var rect = FromWorldRangeToIndexesRange(
                    x - .5f / _cellsPerUnit, Mathf.Min(y1, y2),
                    x + .5f / _cellsPerUnit, Mathf.Max(y1, y2), value);
                list.Add(rect);
            }

            return list;
        }

        public static Rect CalculateAABB(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float minX = Mathf.Min(a.x, b.x, c.x, d.x);
            float minY = Mathf.Min(a.y, b.y, c.y, d.y);
            float maxX = Mathf.Max(a.x, b.x, c.x, d.x);
            float maxY = Mathf.Max(a.y, b.y, c.y, d.y);
            
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
        
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