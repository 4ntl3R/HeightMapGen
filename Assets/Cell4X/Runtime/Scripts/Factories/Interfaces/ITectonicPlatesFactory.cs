namespace Cell4X.Runtime.Scripts.Factories.Interfaces
{
    public interface ITectonicPlatesFactory
    {
        void Inject(System.Random randomizer);
        
        int[,] GenerateTectonicPlates(int fieldSize, int platesAmount);
    }
}
