using System.Collections;
using GridMap;
using UnityEngine;

namespace Tests
{
    public class GridMapTester : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            yield return null;

            var map = FindFirstObjectByType<GridMap.GridMap>();

            var pos = map.FromWorldPositionToIndexes(0f, 0f);
            int size = 3;
            
            var rect = new SetCellsRect(new RectInt(pos.Item1, pos.Item2, size, size), 1);
            
            map.SetCellsAsync(rect);
            
            yield return new WaitForSeconds(3f);

            pos = map.FromWorldPositionToIndexes(-10f, -10f);
            size = 30;
            rect = new SetCellsRect(new RectInt(pos.Item1, pos.Item2, size, size), 1);
            
            map.SetCellsAsync(rect);
        }
    }
}