using System;
using System.Collections.Generic;
using System.Linq;
using Cell4X.Runtime.Scripts.Extensions;
using Cell4X.Runtime.Scripts.Factories.Interfaces;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Factories
{
    public class SimpleTectonicPlatesFactory : ITectonicPlatesFactory
    {
        private const float AdjacentDistance = 1f;
        private static readonly float DiagonalDistance = 1.4f; //Mathf.Sqrt(AdjacentDistance * 2);
        private static readonly float EdgesPrecision = AdjacentDistance *2; 

        private int[,] _result;
        private float[,] _distancesFromCenter;

        private Vector2Int _matrixLength;
        private Queue<Vector2Int> _currentIndexes;

        
        public int[,] GenerateTectonicPlates(int fieldSize, int platesAmount)
        {
            _matrixLength = fieldSize.GetMatrixLengthBySize();
            _result = new int[_matrixLength.x, _matrixLength.y];
            _distancesFromCenter = new float[_matrixLength.x, _matrixLength.y];
            _currentIndexes = new Queue<Vector2Int>();

            GeneratePlatesCenters(platesAmount);
            var stuckCount = 0;
            while (_currentIndexes.Count > 0)
            {
                var currentIndex = _currentIndexes.Dequeue();
                NeighbourFilling(currentIndex, currentIndex.GetAdjacent(_matrixLength), AdjacentDistance);
                NeighbourFilling(currentIndex, currentIndex.GetDiagonal(_matrixLength), DiagonalDistance);
                Debug.Log(_currentIndexes.Count);
                if (++stuckCount > _matrixLength.x*_matrixLength.y)
                {
                    throw new StackOverflowException();
                }
            }
            return _result;
        }

        private void GeneratePlatesCenters(int platesAmount)
        {
            for (var plateIndex = 0; plateIndex < platesAmount; plateIndex++)
            {
                Vector2Int randomIndex;
                List<Vector2Int> neighbours;
                
                do
                {
                    randomIndex = _result.GenerateRandomIndexes();
                    neighbours = new List<Vector2Int>();
                    neighbours.AddRange(randomIndex.GetAdjacent(_matrixLength));
                    neighbours.AddRange(randomIndex.GetDiagonal(_matrixLength));
                    neighbours.Add(randomIndex);
                } 
                while (neighbours.Any(neighbour => _result[neighbour.x, neighbour.y] != 0) );

                _result[randomIndex.x, randomIndex.y] = 2 << plateIndex;
                _currentIndexes.Enqueue(randomIndex);
            }
        }

        private void NeighbourFilling(Vector2Int currentIndex, List<Vector2Int> neighbourIndexes, float distanceToCurrent)
        {
            if (_result[currentIndex.x, currentIndex.y].GetBitCount() != 1)
            {
                return;
            }
            
            foreach (var neighbourIndex in neighbourIndexes)
            {
                if (_result[neighbourIndex.x, neighbourIndex.y] == 0)
                {
                    _result[neighbourIndex.x, neighbourIndex.y] = _result[currentIndex.x, currentIndex.y];
                    _distancesFromCenter[neighbourIndex.x, neighbourIndex.y] =
                        _distancesFromCenter[currentIndex.x, currentIndex.y] + distanceToCurrent;
                    _currentIndexes.Enqueue(neighbourIndex);
                    continue;
                }

                if (_result[neighbourIndex.x, neighbourIndex.y] == _result[currentIndex.x, currentIndex.y])
                {
                    _distancesFromCenter[neighbourIndex.x, neighbourIndex.y] = Mathf.Min(
                        _distancesFromCenter[currentIndex.x, currentIndex.y] + distanceToCurrent, 
                        _distancesFromCenter[neighbourIndex.x, neighbourIndex.y]);
                    continue;
                }

                var newDistanceFromCenter = _distancesFromCenter[currentIndex.x, currentIndex.y] + distanceToCurrent;

                if (newDistanceFromCenter + EdgesPrecision < _distancesFromCenter[neighbourIndex.x, neighbourIndex.y])
                {
                    _result[neighbourIndex.x, neighbourIndex.y] = _result[currentIndex.x, currentIndex.y];
                    _distancesFromCenter[neighbourIndex.x, neighbourIndex.y] = newDistanceFromCenter;
                    continue;
                }
                    
                var oldIntersectingPlanesCount = _result[neighbourIndex.x, neighbourIndex.y].GetBitCount();

                if (Mathf.Abs(newDistanceFromCenter - _distancesFromCenter[neighbourIndex.x, neighbourIndex.y]) <
                    EdgesPrecision * oldIntersectingPlanesCount)
                {
                    _result[neighbourIndex.x, neighbourIndex.y] |= _result[currentIndex.x, currentIndex.y];
                    _distancesFromCenter[neighbourIndex.x, neighbourIndex.y] =
                        Mathf.Min(_distancesFromCenter[neighbourIndex.x, neighbourIndex.y], newDistanceFromCenter);
                }
            }
        }

        private void TestDebugShow()
        {
            Debug.Log("Distances start \n \n \n");
            for (var i = _result.GetLength(1) - 1; i >= 0; i--)
            {
                var currentString = "";
                for (var j = 0; j < _result.GetLength(0); j++)
                {
                    currentString += Mathf.RoundToInt(_distancesFromCenter[j, i] * 10) + " ";
                }
                Debug.Log(currentString);
            }
            
            Debug.Log("\n \n \n Values start \n \n \n");
            
            for (var i = _result.GetLength(1) - 1; i >= 0; i--)
            {
                var currentString = "";
                for (var j = 0; j < _result.GetLength(0); j++)
                {
                    currentString += _result[j, i] + " ";
                }
                Debug.Log(currentString);
            }
        }
    }
}
