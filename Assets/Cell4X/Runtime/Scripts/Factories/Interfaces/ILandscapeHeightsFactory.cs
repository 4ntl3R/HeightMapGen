using UnityEngine;

namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface ILandscapeHeightsFactory
    {
        float[,] CreateLandscape(float[,] preset, float roughness, float decreaseOverTime, int size);
        void Inject(System.Random randomizer);
    }
}
