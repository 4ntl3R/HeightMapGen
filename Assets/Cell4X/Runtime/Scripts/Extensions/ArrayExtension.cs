using UnityEngine;
using UnityEngine.UIElements;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class ArrayExtension
    {
        private const int XDimensionIndex = 0;
        private const int YDimensionIndex = 1;

        public static Vector2Int GenerateRandomIndexes(this int[,] targetMatrix)
        {
            var xIndex = Random.Range(0, targetMatrix.GetLength(XDimensionIndex));
            var yIndex = Random.Range(0, targetMatrix.GetLength(YDimensionIndex));

            return new Vector2Int(xIndex, yIndex);
        }

        public static Vector2Int GetMatrixLengthBySize(this int size)
        {
            var value = (2 << size - 1) + 1;
            return new Vector2Int(value, value);
        }
        
        public static int GetSizeByLength(this Vector2Int lenght)
        {
            return lenght.IsLengthCorrect() ? Mathf.RoundToInt(Mathf.Log(lenght.x, 2)) : -1;
        }

        public static bool IsLengthCorrect(this Vector2Int size)
        {
            return size.x == size.y && ((size.x - 1) & (size.x - 2)) == 0;
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
    }
}
