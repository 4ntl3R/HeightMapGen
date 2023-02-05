using System.Collections.Generic;
using TMPro;

namespace Cell4X.Runtime.Scripts
{
    public class FontSizeMultiManager
    {
        private readonly List<TextMeshProUGUI> _textComponents;

        public FontSizeMultiManager(List<TextMeshProUGUI> textComponents)
        {
            _textComponents = textComponents;
        }

        public void AutoSize()
        {
            var minSize = _textComponents[0].fontSize;
            
            foreach (var textComponent in _textComponents)
            {
                textComponent.enableAutoSizing = true;
                
                var currentSize = textComponent.fontSize;
                minSize = currentSize < minSize ? currentSize : minSize;
            }

            foreach (var textComponent in _textComponents)
            {
                textComponent.enableAutoSizing = false;
                textComponent.fontSize = minSize;
            }
        }
    }
}
