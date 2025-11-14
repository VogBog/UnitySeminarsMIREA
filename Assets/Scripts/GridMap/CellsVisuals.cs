using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using GridMap.Tiles;
using Pool;
using UnityEngine;

namespace GridMap
{
    public class CellsVisuals : MonoBehaviour
    {
        private GridMap _gridMap;
        private ObjectPool _pool;
        private MapObjects _mapObjects;
        
        private readonly Dictionary<Vector2Int, Stack<(Component, Type)>> _chunks = new();
        private readonly Queue<Vector2Int> _jobs = new();
        private bool _jobsProcessing = false;
        
        private void Start()
        {
            _gridMap = this.FindFirstObjectByTypeOrException<GridMap>();
            _pool = this.FindFirstObjectByTypeOrException<ObjectPool>();
            _mapObjects = this.FindFirstObjectByTypeOrException<MapObjects>();
            _gridMap.ChunkChanged += OnChunkChanged;
        }

        private void ClearChunk(Vector2Int chunk)
        {
            if (!_chunks.TryGetValue(chunk, out var stack))
                return;

            while (stack.Count > 0)
            {
                var (component, type) = stack.Pop();
                if (component is ITile tile)
                {
                    tile.Hided += OnComponentHided;
                    tile.OnHide();
                }
                else
                {
                    _pool.Despawn(component, type);
                }
            }
        }

        private void FillChunk(Vector2Int chunk)
        {
            _chunks.TryAdd(chunk, new Stack<(Component, Type)>());
            var stack = _chunks[chunk];
            
            foreach (var (xIndex, yIndex, value) in _gridMap.GetChunkIEnumerator(chunk))
            {
                if(value == 0)
                    continue;
                
                var (x, y) = _gridMap.FromIndexesToWorldPosition(xIndex, yIndex);
                var type = _mapObjects.ByteToType(value);
                
                if(type == null)
                    continue;

                _pool.Spawn(new Vector3(x, 0, y), Quaternion.identity, type, obj =>
                {
                    float scale = 1f / _gridMap.CellsPerUnit;
                    obj.transform.localScale = new Vector3(scale, 1f, scale);
                
                    stack.Push((obj, type));

                    if (obj is ITile tile)
                    {
                        tile.OnShow();
                    }
                });
            }
        }

        private void OnChunkChanged(Vector2Int chunk)
        {
            if(!_jobs.TryPeek(out var last) || chunk != last)
                _jobs.Enqueue(chunk);
            
            if (!_jobsProcessing)
            {
                _jobsProcessing = true;
                StartCoroutine(ProcessJobsRoutine());
            }
        }

        private IEnumerator ProcessJobsRoutine()
        {
            _jobsProcessing = true;

            while (_jobs.Count > 0)
            {
                yield return new WaitForFixedUpdate();
                
                var chunk = _jobs.Dequeue();
                ClearChunk(chunk);
                FillChunk(chunk);
            }
            
            _jobsProcessing = false;
        }

        private void OnComponentHided(Component component, ITile tile)
        {
            tile.Hided -= OnComponentHided;
            _pool.Despawn(component, component.GetType());
        }
    }
}