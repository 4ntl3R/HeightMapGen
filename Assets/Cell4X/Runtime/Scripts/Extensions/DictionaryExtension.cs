using System.Collections.Generic;

namespace Cell4X.Runtime.Scripts.Extensions
{
    public static class DictionaryExtension 
    {
        public static List<SerializablePair<T1, T2>> ToSerializablePairs<T1, T2>(this Dictionary<T1, T2> target)
        {
            var result = new List<SerializablePair<T1, T2>>();
            foreach (var pair in target)
            {
                result.Add(new SerializablePair<T1, T2>(pair.Key, pair.Value));
            }

            return result;
        }

        public static Dictionary<T1, T2> ToDictionary<T1, T2>(this List<SerializablePair<T1, T2>> target)
        {
            var result = new Dictionary<T1, T2>();
            foreach (var pair in target)
            {
                result.Add(pair.First, pair.Second);
            }

            return result;
        }
    }
}
