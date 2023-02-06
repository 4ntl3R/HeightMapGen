using System;
using Cell4X.Runtime.Scripts.Extensions;
using Cell4X.Runtime.Scripts.Factories;
using Cell4X.Runtime.Scripts.Factories.Interfaces;

namespace Cell4X.Runtime.Scripts.Models
{
    public class DiamondSquarePlatesModel
    {
        public event Action<int[,]> HeightsGenerated;
        
        private readonly float[] edgeValues = new[] { -4f, 4f, 1f, -1f };

        private readonly ITectonicPlatesFactory _tectonicPlates;
        private readonly TectonicHeightGenerator _tectonicHeights;
        private readonly DiamondSquareDownscaleFactory _downscaler;
        private readonly ILandscapeHeightsFactory _diamondSquare;
        private readonly IHeightsLayersMergeFactory _merger;

        public DiamondSquarePlatesModel(ITectonicPlatesFactory tectonicPlates, 
            TectonicHeightGenerator tectonicHeights, 
            DiamondSquareDownscaleFactory downscaler, 
            ILandscapeHeightsFactory diamondSquare, 
            IHeightsLayersMergeFactory merger)
        {
            _tectonicPlates = tectonicPlates;
            _tectonicHeights = tectonicHeights;
            _downscaler = downscaler;
            _diamondSquare = diamondSquare;
            _merger = merger;
        }

        public void Generate(GenerationParameters parameters)
        {
            var random = new Random(parameters.Seed);

            var plates = _tectonicPlates.GenerateTectonicPlates(random, parameters.FieldSize, parameters.PlatesCount);
            var heightsPlates = _tectonicHeights
                .GenerateHeightsFromPlates(random, parameters.PlatesRandomAmplitude, parameters.PlatesRandomRatio, 
                    plates, parameters.DecreaseOverRangePlates, edgeValues)
                .SmoothArray();
            
            var downscaledPlates = _downscaler.DownscaleMatrix(heightsPlates, parameters.PlatesDownscale);
            
            var heightsDS = _diamondSquare.CreateLandscape(random, downscaledPlates, parameters.RoughnessDS, 
                parameters.DecreaseOverRangeDS, parameters.FieldSize);

            var mergedHeights = _merger.MergeLayers(heightsPlates, heightsDS).SmoothArray(parameters.MergeSmoothSteps);
            
            HeightsGenerated?.Invoke(mergedHeights);
        }
    }
}
