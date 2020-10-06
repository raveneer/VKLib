using System;
using UnityEngine;
using UnityEngine.UI;

namespace VKLib
{
    [ExecuteInEditMode]
    [Serializable]
    public class UIText : MonoBehaviour
    {
        public Color Color = Color.white;
        public Text TmpUiText;

        public string Text
        {
            get => _cachedString;
            set
            {
                if (_cachedString != value)
                {
                    _cachedString = value;
                    TmpUiText.text = _cachedString;
                }
            }
        }
        private Color _cachedColor;
        private string _cachedString = "";

        private void Awake()
        {
        }

        public void SetColor(Color color)
        {
            if (_cachedColor != color)
            {
                _cachedColor = color;
                TmpUiText.color = color;
                Color = color;
            }
        }

        private void SetSizeZero()
        {
            var rect = gameObject.GetComponent<RectTransform>();
            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = new Vector2(0, 0);
        }
    }
}