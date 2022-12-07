using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "ValueGenerationData", menuName = "Cell4X/ValueGenerationData"), ]
    public class ValueGenerationData<T> : ScriptableObject
    {
        [SerializeField] 
        private List<SerializablePair<T, int>> valueAndRelativeFrequency;

        [SerializeField] 
        private List<float> borderValues;

        private bool _isInitiated = false;

        public T GetRandomValue(System.Random randomizer = null)
        {
            randomizer ??= new System.Random();
            if (!_isInitiated)
            {
                Initiate();
                _isInitiated = true;
            }

            return valueAndRelativeFrequency[FindIndex((float)randomizer.NextDouble())].First;
        }

        private void Initiate()
        {
            var frequencySum = valueAndRelativeFrequency.Sum(pair => pair.Second);
            var currentSum = 0;
            
            borderValues.Add(0);
            for (var i = 0; i < valueAndRelativeFrequency.Count; i++)
            {
                currentSum += valueAndRelativeFrequency[i].Second;
                borderValues.Add((float)currentSum/frequencySum);
            }
        }

        private int FindIndex(float percentValue)
        {
            for (var i = 0; i < valueAndRelativeFrequency.Count - 1; i++)
            {
                if (borderValues[i] < percentValue && borderValues[i + 1] > percentValue)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
