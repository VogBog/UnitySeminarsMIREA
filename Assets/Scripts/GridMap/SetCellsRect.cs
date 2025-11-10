using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridMap
{
    public readonly struct SetCellsRect
    {
        public readonly RectInt Rect;
        public readonly byte Value;

        public SetCellsRect(RectInt rect, byte value)
        {
            Rect = rect;
            Value = value;
        }

        public (int, int, int, int, byte) ToRawData()
        {
            return (Rect.xMin, Rect.yMin, Rect.width, Rect.height, Value);
        }

        public static SetCellsRect FromRawData(int xMin, int yMin, int width, int height, byte value)
        {
            var rect = new RectInt(xMin, yMin, width, height);
            return new(rect, value);
        }

        public static (int[], int[], int[], int[], byte[]) ToRawDataArray(IEnumerable<SetCellsRect> rects)
        {
            var list = new List<(int, int, int, int, byte)>();
            foreach (var rect in rects)
            {
                var data = rect.ToRawData();
                list.Add(data);
            }

            int[] x = list.Select(i => i.Item1).ToArray();
            int[] y = list.Select(i => i.Item2).ToArray();
            int[] width = list.Select(i => i.Item3).ToArray();
            int[] height = list.Select(i => i.Item4).ToArray();
            byte[] values = list.Select(i => i.Item5).ToArray();
            
            return (x, y, width, height, values);
        }

        public static SetCellsRect[] FromRawDataArray(int[] x, int[] y, int[] widths, int[] heights, byte[] values)
        {
            var result = new SetCellsRect[Mathf.Min(x.Length, y.Length, widths.Length, heights.Length, values.Length)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = FromRawData(x[i], y[i], widths[i], heights[i], values[i]);
            }

            return result;
        }
    }
}