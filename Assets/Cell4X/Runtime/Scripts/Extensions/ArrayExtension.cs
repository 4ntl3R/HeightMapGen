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
            return length.IsLengthCorrect() ? Mathf.RoundToInt(Mathf.Log(length.x, 2)) : -1;
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
    }
}
