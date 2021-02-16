using UnityEngine;

namespace VKLib.VKLib.UI
{
    public class UnityRectHelper
    {
        /// <summary>
        /// 정말 이상하군... 4방향 확장에서 top을 지정하면 음수로 들어가린다. 정말 이유를 알 수 없다.
        /// </summary>
        public static void SetRectTop(RectTransform rect, float distance)
        {
            rect.offsetMax = new Vector2(rect.offsetMax.x, -distance);
        }

        public static void SetRectBottom(RectTransform rect, float distance)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, distance);
        }

        
        public static void SetRectWidth(RectTransform rect, float width)
        {
            rect.sizeDelta  = new Vector2(width, rect.sizeDelta.y);
        }

        
        public static void SetRectHeight(RectTransform rect, float height)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        }
    }
}