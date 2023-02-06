using System;
using UnityEngine;

namespace Cell4X.Runtime.Scripts
{
    [Serializable]
    public class SerializablePair<T1, T2>
    {
        [SerializeField] 
        private T1 first;

        [SerializeField] 
        private T2 second;
        
        public T1 First => first;
        
        public T2 Second => second;

        public SerializablePair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
