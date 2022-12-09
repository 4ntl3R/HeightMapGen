using System;

namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface IHeightsLayersMergeFactory
    {
        public int[,] MergeLayers(params float?[][,] layers);
    }
}
