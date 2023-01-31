using Cell4X.Runtime.Scripts.Extensions;

namespace Cell4X.Runtime.Scripts.Factories
{
    public class DiamondSquareDownscaleFactory
    {
        private float?[,] _targetMatrix;

        public float[,] DownscaleMatrix(float?[,] targetMatrix, int sizeReduction)
        {
            _targetMatrix = targetMatrix;
            
            var resultSize = _targetMatrix.GetMatrixSize().GetMatrixSizeByLength() - sizeReduction;
            var resultLength = resultSize.GetMatrixLengthBySize();
            var result = new float[resultLength.x, resultLength.y];

            var sizeCells = sizeReduction.GetMatrixLengthBySize();
            
            for (var resultX = 0; resultX < resultLength.x; resultX++)
            {
                for (var resultY = 0; resultY < resultLength.y; resultY++)
                {
                    result[resultX, resultY] = DownscaleCellValue(resultX * (sizeCells.x - 1), resultY * (sizeCells.y - 1) , sizeCells.x - 2);
                }
            }

            return result;
        }

        private float DownscaleCellValue(int targetX, int targetY, int range)
        {
            if (_targetMatrix[targetX, targetY] is null)
                return 0;
            var (xMin, xMax, yMin, yMax) = _targetMatrix.GetValidNeighbours(targetX, targetY, range);
            var divider = 0f;
            var sum = 0f;

            for (var x = xMin; x < xMax + 1; x++)
            {
                for (var y = yMin; y < yMax + 1; y++)
                {
                    if (_targetMatrix[x, y] is null)
                    {
                        continue;
                    }
                    
                    var multiplier = x.Equals(targetX) && x.Equals(targetY) ? 4 
                        : x.Equals(targetX) || x.Equals(targetY) ? 2 
                        : 1;

                    sum += (_targetMatrix[x, y].Value * multiplier);
                    divider += multiplier;
                }
            }

            return sum / divider;
        }
    }
}
