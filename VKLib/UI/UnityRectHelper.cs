using UnityEngine;

namespace VKLib.VKLib.UI
{
    public class UnityRectHelper
    {
        /// <summary>
        /// ���� �̻��ϱ�... 4���� Ȯ�忡�� top�� �����ϸ� ������ ������. ���� ������ �� �� ����.
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