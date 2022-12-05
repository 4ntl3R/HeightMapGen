using System.Collections.Generic;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class IntegerBitwiseExtension 
    {
        public static int GetBitCount(this int target)
        {
            var count = 0;
            while (target != 0)
            {
                count++;
                target &= target - 1;
            }
            return count;
        }

        public static List<int> GetBitDigitPositions(this int target)
        {
            var result = new List<int>();
            var counter = 0;
            while (result.Count < target.GetBitCount())
            {
                var currentChecked = 2 << counter;
                if ((currentChecked & target) != 0)
                {
                    result.Add(counter);
                }

                counter++;
            }

            return result;
        }
    }
}
