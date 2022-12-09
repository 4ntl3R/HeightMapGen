using System;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class RandomExtension 
    {
        public static float NextFloatInRange(this Random randomizer, float absRange)
        {
            return (float)randomizer.NextDouble() * 2 * absRange - absRange;
        }
    }
}
