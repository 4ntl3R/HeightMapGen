using System;
using System.Collections.Generic;
using System.Linq;
using Cell4X.Runtime.Scripts.Extensions;
using Cell4X.Runtime.Scripts.Factories.Interfaces;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Factories
{
    public class SimpleLayerMerger : IHeightsLayersMergeFactory
    {
        private static float MinLerp = 0.2f; 
        private static float MaxLerp = 0.5f; 

        
        public int[,] MergeLayers(params float?[][,] layers)
        {
            if (layers.Select(x => x.GetMatrixSize()).Distinct().Count() != 1)
            {
                throw new Exception("Layers is different sized");
            }
            
            var layersLength = layers[0].GetMatrixSize();
            var result = new int[layersLength.x, layersLength.y];
            
            for (var x = 0; x < layersLength.x; x++)
            {
                for (var y = 0; y < layersLength.y; y++)
                {
                    result[x, y] = MergeCells(layers.Select(layer => layer[x, y]));
                }
            }

            return result;
        }

        private int MergeCells(IEnumerable<float?> cells)
        {
            var significantCells = cells
                .Where(value => !(value is null))
                .Select(value => (float)value)
                .ToList();

            return significantCells.Count switch
            {
                0 => throw new ArgumentNullException(),
                1 => Mathf.RoundToInt(significantCells[0]),
                _ => Mathf.RoundToInt(MergeValues(significantCells))
            };
        }

        private float MergeValues(List<float> values)
        {
            var current = values[values.Count - 1];
            for (var i = values.Count - 2; i >= 0; i--)
            {
                var max = Mathf.Abs(values[i]) > Mathf.Abs(current) ? values[i] : current;
                var min = Mathf.Abs(values[i]) > Mathf.Abs(current) ? current : values[i];
                current = Mathf.Lerp(max, min, 
                    Mathf.Min(MaxLerp, Mathf.Max(MinLerp, Mathf.Abs(min) + 1/Mathf.Abs(max) + 1)));
            }

            return current;
        }
    }
}
