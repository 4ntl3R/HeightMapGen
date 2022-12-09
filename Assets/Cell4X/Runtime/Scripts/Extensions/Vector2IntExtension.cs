using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class Vector2IntExtension
    {
        public static List<Vector2Int> GetAdjacent(this Vector2Int target, Vector2Int arrayLength)
        {
            var result = new List<Vector2Int>
            {
                new Vector2Int(target.x, target.y - 1),
                new Vector2Int(target.x, target.y + 1),
                new Vector2Int(target.x - 1, target.y),
                new Vector2Int(target.x + 1, target.y)
            };

            return result.FindAll(x => x.IsValid(arrayLength));
        }
        
        public static List<Vector2Int> GetDiagonal(this Vector2Int target, Vector2Int arrayLength)
        {
            var result = new List<Vector2Int>
            {
                new Vector2Int(target.x - 1, target.y - 1),
                new Vector2Int(target.x + 1, target.y + 1),
                new Vector2Int(target.x - 1, target.y + 1),
                new Vector2Int(target.x + 1, target.y - 1)
            };

            return result.FindAll(x => x.IsValid(arrayLength));
        }
        
        public static Vector2Int GetLoopedCoords(this Vector2Int value, Vector2Int max)
        {
            return new Vector2Int((value.x + max.x) % max.x, (value.y + max.y) % max.y);
        }

        public static float GetAverageInCells(this List<Vector2Int> coords, float[,] matrix)
        {
            return coords
                .FindAll(x => x.IsValid(matrix))
                .Select(coord => matrix[coord.x, coord.y])
                .Average();
        }
        
        public static bool IsValid<T>(this Vector2Int target, T[,] array)
        {
            return target.IsValid(array.GetMatrixSize());
        }

        public static bool IsValid(this Vector2Int target, Vector2Int arrayLength)
        {
            return (target.x >= 0) && (target.y >= 0) && (target.x < arrayLength.x) && (target.y < arrayLength.y);
        }
    }
}
