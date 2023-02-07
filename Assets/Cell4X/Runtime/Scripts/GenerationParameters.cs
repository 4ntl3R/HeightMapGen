using System;
using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Enums;
using UnityEngine;

namespace Cell4X.Runtime.Scripts
{
    public struct GenerationParameters
    {
        public int Seed { get; }
        public int FieldSize { get; }
        public float DecreaseOverRangePlates { get; }
        public float PlatesRandomAmplitude { get; }
        public float PlatesRandomRatio { get; }
        public float RoughnessDS { get; }
        public float DecreaseOverRangeDS { get; }
        public int PlatesDownscale { get; }
        public int MergeSmoothSteps { get; }
        public int PlatesCount { get; }
        

        public GenerationParameters(List<SerializablePair<ParameterType, float>> parameters)
        {
            Seed = PlatesCount = MergeSmoothSteps = PlatesDownscale = 0;
            DecreaseOverRangePlates = PlatesRandomAmplitude = PlatesRandomRatio = RoughnessDS = DecreaseOverRangeDS = 0;
            FieldSize = 6;
            
            foreach (var parameter in parameters)
            {
                switch (parameter.First)
                {
                    case ParameterType.Seed:
                        Seed = Mathf.RoundToInt(parameter.Second);
                        break;
                    case ParameterType.FieldSize:
                        FieldSize = Mathf.RoundToInt(parameter.Second);
                        break;
                    case ParameterType.NoiseLevel:
                        RoughnessDS = parameter.Second;
                        break;
                    case ParameterType.NoisePower:
                        PlatesDownscale = Mathf.RoundToInt(parameter.Second);
                        break;
                    case ParameterType.NoiseReduction:
                        DecreaseOverRangeDS = parameter.Second;
                        break;
                    case ParameterType.PlatesNumber:
                        PlatesCount = Mathf.RoundToInt(parameter.Second);
                        break;
                    case ParameterType.SmoothingLevel:
                        MergeSmoothSteps = Mathf.RoundToInt(parameter.Second);
                        break;
                    case ParameterType.PlateRangeRandom:
                        PlatesRandomAmplitude = parameter.Second;
                        break;
                    case ParameterType.PlatesBorderSize:
                        DecreaseOverRangePlates = parameter.Second;
                        break;
                    case ParameterType.PlatesHeightRandom:
                        PlatesRandomRatio = parameter.Second;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
