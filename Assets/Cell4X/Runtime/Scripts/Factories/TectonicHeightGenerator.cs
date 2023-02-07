using System;
using System.Collections.Generic;
using System.Linq;
using Cell4X.Runtime.Scripts.Extensions;
using UnityEngine;
using Random = System.Random;

namespace Cell4X.Runtime.Scripts.Factories
{
    public class TectonicHeightGenerator
    {
        private static readonly float DiagonalToAdjacentRatio =   ArrayExtension.DiagonalDistance
                                                                / ArrayExtension.AdjacentDistance; 
        
        private Random _randomizer;
        private float _randomAmplitude;
        private float _randomRatio;
        
        private int[,] _platesMatrix;
        private float?[,] _result;
        private Dictionary<int, float> _edgesHeights;
        private List<Vector2Int> _edgesCoords;

        private Vector2Int _matrixSize;

        public float?[,] GenerateHeightsFromPlates(Random randomizer, float randomAmplitude, float randomRatio, 
            int[,] platesMatrix, float decreaseOverRange, float[] edgeValues)
        {
            _randomizer = randomizer;
            _randomAmplitude = randomAmplitude;
            _randomRatio = randomRatio;
            _platesMatrix = platesMatrix;
            _matrixSize = platesMatrix.GetMatrixSize();
            _result = new float?[_matrixSize.x, _matrixSize.y];
            FillEdges(edgeValues);
            FillNearby(decreaseOverRange);
            return _result;
        }

        private void FillEdges(float[] edgeValues)
        {
            _edgesHeights = new Dictionary<int, float>();
            _edgesCoords = new List<Vector2Int>();
            for (var x = 0; x < _matrixSize.x; x++)
            {
                for (var y = 0; y < _matrixSize.y; y++)
                {
                    var plateIndex = _platesMatrix[x, y];
                    
                    if (plateIndex.GetBitCount() == 1)
                    {
                        continue;
                    }
                    
                    _edgesCoords.Add(new Vector2Int(x, y));

                    if (!_edgesHeights.ContainsKey(plateIndex))
                    {
                        _edgesHeights.Add(plateIndex, edgeValues.GetRandomElement(_randomizer));
                    }

                    _result[x, y] = _edgesHeights[plateIndex];
                }
            }
        }

        private void FillNearby(float decreaseOverRange)
        {
            var affectedCells = new int[_matrixSize.x, _matrixSize.y];
            var cellQueue = new Queue<Vector2Int>(_edgesCoords);

            var stuckCounter = 0;
            var nextWave = new HashSet<Vector2Int>();
            RandomizeByCoords(_edgesCoords, _randomAmplitude);
            while (cellQueue.Count > 0 || nextWave.Count > 0)
            {
                if (cellQueue.Count == 0)
                {
                    cellQueue = new Queue<Vector2Int>(nextWave);
                    _edgesCoords.AddRange(nextWave);
                    _randomAmplitude /= _randomRatio;
                    RandomizeByCoords(nextWave, _randomAmplitude);
                    nextWave = new HashSet<Vector2Int>();
                }
                
                var currentCoords = cellQueue.Dequeue();
                var currentHeight = _result[currentCoords.x, currentCoords.y];
                if (currentHeight is null)
                {
                    throw new ArgumentNullException();
                }
                if (Mathf.Abs((float)currentHeight) < decreaseOverRange)
                {
                    continue;
                }
                
                var adjacentValue = GetDecreasedValue((float)currentHeight, decreaseOverRange);
                var adjacentCoords = currentCoords
                    .GetAdjacent(_matrixSize)
                    .Where(coords => !_edgesCoords.Contains(coords))
                    .ToList();
                
                HandleNeighbours(adjacentCoords, nextWave, affectedCells, adjacentValue);
                
                var diagonalValue = GetDecreasedValue((float)currentHeight, decreaseOverRange * DiagonalToAdjacentRatio);
                var diagonalCoords = currentCoords
                    .GetDiagonal(_matrixSize)
                    .Where(coords => !_edgesCoords.Contains(coords))
                    .ToList();
                
                HandleNeighbours(diagonalCoords, nextWave, affectedCells, diagonalValue);

                if (stuckCounter++ > _matrixSize.x * _matrixSize.y)
                {
                    throw new StackOverflowException();
                }
            }
        }

        private float GetDecreasedValue(float target, float decrease)
        {
            return (Mathf.Abs(target) - decrease) * Mathf.Sign(target);
        }

        private void HandleNeighbours(List<Vector2Int> neighbours, ICollection<Vector2Int> nextWave, 
            int[,] affectedCells, float originValue)
        {
            foreach (var neighbour in neighbours)
            {
                nextWave.Add(neighbour);

                affectedCells[neighbour.x, neighbour.y]++;
                if (affectedCells[neighbour.x, neighbour.y] == 1)
                {
                    _result[neighbour.x, neighbour.y] = originValue;
                    continue;
                }

                _result[neighbour.x, neighbour.y] /= affectedCells[neighbour.x, neighbour.y] - 1;
                _result[neighbour.x, neighbour.y] += originValue;
                _result[neighbour.x, neighbour.y] /= affectedCells[neighbour.x, neighbour.y];
            }
        }

        private void RandomizeByCoords(IEnumerable<Vector2Int> target, float randomMultiplier)
        {
            foreach (var coords in target)
            {
                _result[coords.x, coords.y] += _randomizer.NextFloatInRange(randomMultiplier);
            }
        }
    }
}
