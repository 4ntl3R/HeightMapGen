using System;
using UnityEngine;

namespace Cell4X.Runtime.Scripts
{
    [Serializable]
    public class SerializablePair<T1, T2>
    {
        [SerializeField] 
        private T1 _first;

        [SerializeField] 
        private T2 _second;
        
        public T1 First => _first;
        
        public T2 Second => _second;
    }
}
