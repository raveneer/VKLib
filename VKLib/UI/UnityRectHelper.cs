using UnityEngine;

namespace VKLib.VKLib.UI
{
    public class UnityRectHelper
    {
        public static void SetRectTop(RectTransform rect, float distance)
        {
            rect.offsetMax = new Vector2(rect.offsetMax.x, distance);
        }

        public static void SetRectBottom(RectTransform rect, float distance)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, distance);
        }
    }
}