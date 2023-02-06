using System;

namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface ITectonicPlatesFactory
    {
        int[,] GenerateTectonicPlates(Random randomizer, int fieldSize, int platesAmount);
    }
}
