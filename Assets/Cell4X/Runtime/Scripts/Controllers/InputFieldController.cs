using System;
using System.Collections.Generic;
using Cell4X.Runtime.Scripts.Extensions;
using TMPro;
using UnityEngine;
using ParameterType = Cell4X.Runtime.Scripts.Enums.ParameterType;

namespace Cell4X.Runtime.Scripts.Controllers
{
    public class InputFieldController : MonoBehaviour
    {
        public event Action<GenerationParameters> GenerationButtonPressed;
        
        [SerializeField] 
        private List<SerializablePair<ParameterType, TMP_InputField>> inputParameters;

        private Dictionary<ParameterType, TMP_InputField> _inputParametersDictionary;

        private Dictionary<ParameterType, float> _currentParameterValues;

        private bool _isInitiated = false;

        public void OnInputFieldUpdate(string fieldIdentifier)
        {
            TryInitiate();
            
            var parameterType = (ParameterType)int.Parse(fieldIdentifier);
            var newValue = float.Parse(_inputParametersDictionary[parameterType].text);

            if (!_currentParameterValues.ContainsKey(parameterType))
            {
                _currentParameterValues.Add(parameterType, newValue);
                return;
            }

            _currentParameterValues[parameterType] = newValue;
        }

        public void Generate()
        {
            TryInitiate();
            if (_currentParameterValues.Count < inputParameters.Count)
            {
                return;
            }

            var parameters = new GenerationParameters(_currentParameterValues.ToSerializablePairs());
            GenerationButtonPressed?.Invoke(parameters);
        }

        private void TryInitiate()
        {
            if (_isInitiated)
            {
                return;
            }
            _isInitiated = true;

            _currentParameterValues = new Dictionary<ParameterType, float>();
            _inputParametersDictionary = new Dictionary<ParameterType, TMP_InputField>();
            foreach (var inputParameter in inputParameters)
            {
                _inputParametersDictionary.Add(inputParameter.First, inputParameter.Second);
            }
        }
    }
}
