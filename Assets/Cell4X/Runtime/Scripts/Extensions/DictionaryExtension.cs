using System.Collections.Generic;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class DictionaryExtension 
    {
        public static List<SerializablePair<T1, T2>> ToSerializablePairs<T1, T2>(this Dictionary<T1, T2> target)
        {
            var result = new List<SerializablePair<T1, T2>>();
            foreach (var kvp in target)
            {
                result.Add(new SerializablePair<T1, T2>(kvp.Key, kvp.Value));
            }

            return result;
        }
    }
}
