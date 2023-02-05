using System;
using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Enums;
using TMPro;
using UnityEngine;

namespace Cell4X.Runtime.Scripts.Controllers
{
    public class InputFieldController : MonoBehaviour
    {
        public event Action<ParameterType, float> InputParameterUpdate;
        
        [SerializeField] 
        private List<SerializablePair<ParameterType, TMP_InputField>> inputParameters;

        private Dictionary<ParameterType, TMP_InputField> _inputParametersDictionary;

        private bool _isInitiated = false;

        public void OnInputFieldUpdate(string fieldIdentifier)
        {
            TryInitiate();
            
            var parameterType = (ParameterType)int.Parse(fieldIdentifier);
            var newValue = float.Parse(_inputParametersDictionary[parameterType].text);
            
            Debug.Log(parameterType);
            InputParameterUpdate?.Invoke(parameterType, newValue);
        }

        private void TryInitiate()
        {
            if (_isInitiated)
            {
                return;
            }
            _isInitiated = true;

            _inputParametersDictionary = new Dictionary<ParameterType, TMP_InputField>();
            foreach (var inputParameter in inputParameters)
            {
                _inputParametersDictionary.Add(inputParameter.First, inputParameter.Second);
            }
        }
    }
}
