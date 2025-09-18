using System;
using System.Collections;
using System.Collections.Generic;
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
            _map[x, y] = c;
            ChunkChanged?.Invoke(new(x / _chunkSize, y / _chunkSize));
        }

        private void SetCells(SetCellsRect rect)
        {
            int minX = (int)rect.Rect.xMin;
            int minY = (int)rect.Rect.yMin;
            int maxX = (int)rect.Rect.xMax;
            int maxY = (int)rect.Rect.yMax;
            _cellsBuffer.Clear();

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    _map[x, y] = rect.Value;
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

            for (float x = rect.Rect.xMin; x < rect.Rect.xMax; x += _chunkSize)
            {
                for (float y = rect.Rect.yMin; y < rect.Rect.yMax; y += _chunkSize)
                {
                    float maxX = Mathf.Min(x + _chunkSize, rect.Rect.xMax);
                    float maxY = Mathf.Min(y + _chunkSize, rect.Rect.yMax);
                    var newRect = new Rect(x, y, maxX - x, maxY - y);
                        
                    _jobs.Enqueue(new(newRect, rect.Value));
                }
            }
            
            if (!_processingJobs)
            {
                _processingJobs = true;
                StartCoroutine(ProcessJobs());
            }
        }

        public void SetCellsAsync(SetCellsRect[] rects)
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

        public GridMapIEnumerator GetAreaIEnumerator(Rect rect)
            => new GridMapIEnumerator(rect, _map);

        public GridMapIEnumerator GetChunkIEnumerator(Vector2Int chunk)
        {
            int minX = chunk.x * _chunkSize;
            int minY = chunk.y * _chunkSize;

            var rect = new Rect(minX, minY, _chunkSize, _chunkSize);
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