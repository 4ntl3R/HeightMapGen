using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Extensions;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "HeightsColorData", menuName = "Cell4X/HeightsColorData"), ]
    public class HeightColorsData : ScriptableObject
    {
        [SerializeField] 
        private List<SerializablePair<int, Color>> heightColors;

        public Dictionary<int, Color> GetColorDictionary => heightColors.ToDictionary();
    }
}
