using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class EasyFiguresDrawer : MonoBehaviour
    {
        [SerializeField] private Mesh _cube;
        
        private readonly Stack<(Vector3, Vector3, Quaternion, Color, float)> _cubes = new();
        private readonly Stack<(Vector3, Vector3, Quaternion, Color, float)> _cubesBuffer = new();
        
        public static EasyFiguresDrawer Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            DrawCubes();
        }

        private void DrawCubes()
        {
            _cubesBuffer.Clear();
            
            while (_cubes.Count > 0)
            {
                var (center, size, rotation, color, time) = _cubes.Pop();
                Gizmos.color = color;
                Gizmos.DrawWireMesh(_cube, center, rotation, size);
                
                time -= Time.deltaTime;
                if(time > 0f)
                    _cubesBuffer.Push((center, size, rotation, color, time));
            }

            while (_cubesBuffer.Count > 0)
            {
                _cubes.Push(_cubesBuffer.Pop());
            }
        }

        public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, Color color, float time = 1f)
        {
            Instance._cubes.Push((center, size, rotation, color, time));
        }
    }
}