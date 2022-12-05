using UnityEngine;

namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface ITectonicPlatesFactory
    {
        int[,] GenerateTectonicPlates(int fieldSize, int platesAmount);
    }
}
