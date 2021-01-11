using System;
using TMPro;
using UnityEngine;

namespace VKLib.VKLib.UI
{
    [ExecuteInEditMode]
    [Serializable]
    public class UIText : MonoBehaviour
    {
        public TextMeshPro TmpUiText;
        public Color Color { get; private set; } = Color.white;

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

        private string _cachedString = "";

        private void Awake()
        {
        }

        public void SetColor(Color color)
        {
            if (Color != color)
            {
                Color = color;
                TmpUiText.color = color;
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