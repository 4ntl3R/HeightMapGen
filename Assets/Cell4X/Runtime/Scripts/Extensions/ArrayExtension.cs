using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class ArrayExtension
    {
        public const float AdjacentDistance = 1f;
        public static readonly float DiagonalDistance = 1.41f; //Mathf.Sqrt(AdjacentDistance * 2);
        
        private const int XDimensionIndex = 0;
        private const int YDimensionIndex = 1;

        public static Vector2Int GetMatrixSize<T>(this T[,] target)
        {
            return new Vector2Int(target.GetLength(XDimensionIndex), target.GetLength(YDimensionIndex));
        }
        
        public static Vector2Int GenerateRandomIndexes(this int[,] targetMatrix, System.Random randomizer = null)
        {
            randomizer ??= new System.Random();
            var xIndex = randomizer.Next(0, targetMatrix.GetLength(XDimensionIndex));
            var yIndex = randomizer.Next(0, targetMatrix.GetLength(YDimensionIndex));

            return new Vector2Int(xIndex, yIndex);
        }

        public static Vector2Int GetMatrixLengthBySize(this int size)
        {
            var value = (2 << size - 1) + 1;
            return new Vector2Int(value, value);
        }
        
        public static int GetMatrixSizeByLength(this Vector2Int length)
        {
             return length.IsLengthCorrect() ? Mathf.RoundToInt(Mathf.Log(length.x - 1, 2)) : -1;
        }

        public static bool IsLengthCorrect(this Vector2Int size)
        {
            return size.x == size.y && ((size.x - 1) & (size.x - 2)) == 0;
        }

        public static T GetRandomElement<T>(this T[] array, System.Random randomizer)
        {
            randomizer ??= new System.Random();
            return array[randomizer.Next(0, array.Length)];
        }

        public static int[,] SmoothArray(this int[,] target, int steps = 1)
        {
            var targetSize = target.GetMatrixSize(); 
            var result = new int[targetSize.x, targetSize.y];

            for (var x = 0; x < targetSize.x; x++)
            {
                for (var y = 0; y < targetSize.y; y++)
                {
                    result[x, y] = target.SmoothCellValue(x, y, steps);
                }
            }
            return result;
        }
        
        public static float?[,] SmoothArray(this float?[,] target, int steps = 1)
        {
            var targetSize = target.GetMatrixSize(); 
            var result = new float?[targetSize.x, targetSize.y];

            for (var x = 0; x < targetSize.x; x++)
            {
                for (var y = 0; y < targetSize.y; y++)
                {
                    result[x, y] = target.SmoothCellValue(x, y, steps);
                }
            }
            return result;
        }

        public static (int, int, int, int) GetValidNeighbours<T>(this T[,] target, int cellX, int cellY, int range)
        {
            var matrixLength = target.GetMatrixSize();
            
            var xMin = Mathf.Max(cellX - range, 0);
            var xMax = Mathf.Min(cellX + range, matrixLength.x - 1);
            var yMin = Mathf.Max(cellY - range, 0);
            var yMax = Mathf.Min(cellY + range, matrixLength.y - 1);

            return (xMin, xMax, yMin, yMax);
        }
        
        private static int SmoothCellValue(this int[,] target, int cellX, int cellY, int range)
        {
            var (xMin, xMax, yMin, yMax) = target.GetValidNeighbours(cellX, cellY, range);

            var divider = 0f;
            var sum = 0f;

            for (var x = xMin; x < xMax + 1; x++)
            {
                for (var y = yMin; y < yMax + 1; y++)
                {
                    sum += target[x, y];
                    divider++;
                }
            }

            return Mathf.RoundToInt(sum / divider);
        }
        
        private static float? SmoothCellValue(this float?[,] target, int cellX, int cellY, int range)
        {
            var (xMin, xMax, yMin, yMax) = target.GetValidNeighbours(cellX, cellY, range);

            var divider = 0f;
            var sum = 0f;

            if (target[cellX, cellY] is null)
            {
                return null;
            }

            for (var x = xMin; x < xMax + 1; x++)
            {
                for (var y = yMin; y < yMax + 1; y++)
                {
                    if (target[x, y] is null)
                    {
                        continue;
                    }
                    sum += target[x, y].Value;
                    divider++;
                }
            }

            return sum / divider;
        }
        
        
    }
}
