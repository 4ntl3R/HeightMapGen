using System;
using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Extensions;
using Cell4X.Runtime.Scripts.Factories.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Cell4X.Runtime.Scripts.Factories
{
    public class DiamondSquareHeightGenerator : ILandscapeHeightsFactory
    {
        private Random _randomizer;
        private float?[,] _result;
        private Vector2Int _matrixSize;
        private int _fillDistance;
        private int _presetDistance;
        
        public void Inject(Random randomizer)
        {
            _randomizer = randomizer;
        }

        public float?[,] CreateLandscape(float[,] preset, float roughness, float decreaseOverTime, int size)
        {
            _matrixSize = size.GetMatrixLengthBySize();
            var length = _matrixSize.x;
            if (!TryFillPreset(preset, size))
            {
                throw new Exception("Preset size is invalid");
            }

            var currentDelta = roughness / _presetDistance;
            for (var sideLength = (_matrixSize.x - 1)/_presetDistance; sideLength > 1; sideLength /= 2, currentDelta /= decreaseOverTime)
            {
                var halfSide = sideLength / 2;
                for (var x = 0; x < length - 1; x += sideLength) 
                {
                    for (var y = 0; y < length - 1; y += sideLength) 
                    {
                        var coords = new List<Vector2Int>
                        {
                            new Vector2Int(x, y),
                            new Vector2Int(x + sideLength, y),
                            new Vector2Int(x, y + sideLength),
                            new Vector2Int(x + sideLength, y + sideLength),
                        };

                        var average = coords.GetAverageInCells(_result);
                        
                        _result[x + halfSide, y + halfSide] = average + (float)_randomizer.NextDouble() * 2 * currentDelta - currentDelta;
                    }
                }
                for (var x = 0; x < length * 2; x += sideLength) 
                {
                    for (var y = 0; y < length * 2; y += sideLength) 
                    {
                        if (!new Vector2Int(x + halfSide, y).IsValid(_matrixSize))
                        {
                            continue;
                        }
                        
                        var coords = new List<Vector2Int>
                        {
                            new Vector2Int(x, y),
                            new Vector2Int(x + sideLength, y),
                            new Vector2Int(x + halfSide, y + halfSide),
                            new Vector2Int(x + halfSide, y - halfSide),
                        };

                        var average = coords.GetAverageInCells(_result);

                        _result[x + halfSide, y] = average  + (float)_randomizer.NextDouble() * 2 * currentDelta - currentDelta;
                    }
                }
                
                for (var x = 0; x < length + 1; x += sideLength) 
                {
                    for (var y = 0; y < length + 1; y += sideLength) 
                    {
                        if (!new Vector2Int(x, y + halfSide).IsValid(_matrixSize))
                        {
                            continue;
                        }
                        
                        var coords = new List<Vector2Int>
                        {
                            new Vector2Int(x, y),
                            new Vector2Int(x, y + sideLength),
                            new Vector2Int(x + halfSide, y + halfSide),
                            new Vector2Int(x - halfSide, y + halfSide)
                        };

                        var average = coords.GetAverageInCells(_result);

                        _result[x, y + halfSide] = average  + (float)_randomizer.NextDouble() * 2 * currentDelta - currentDelta;
                    }
                }
            }
            return _result;
        }

        private bool TryFillPreset(float[,] preset, int matrixSize)
        {
            _result = new float?[_matrixSize.x, _matrixSize.y];
            
            if (!preset.GetMatrixSize().IsLengthCorrect())
            {
                return false;
            }

            var presetSize = preset.GetMatrixSize().GetMatrixSizeByLength();
            
            _fillDistance = 2 << (matrixSize - presetSize);
            _presetDistance = Mathf.RoundToInt(Mathf.Pow(2, presetSize - 1));

            var matrixLength = preset.GetMatrixSize();

            for (var x = 0; x < matrixLength.x; x++)
            {
                for (var y = 0; y < matrixLength.y; y++)
                {
                    _result[x * _fillDistance, y * _fillDistance] = preset[x, y];
                }
            }

            return true;
        }
    }
}
