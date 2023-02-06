using UnityEngine;

namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface ILandscapeHeightsFactory
    {
        float?[,] CreateLandscape(System.Random random, float[,] preset, float roughness, float decreaseOverTime, int size);
    }
}
