using UnityEngine;
using VKLib.Native;
using Zenject;

namespace VKLib.VKLib.UI
{
    ///<summary>
    /// 애드몹 스마트배너에 의해서 높이를 조절하는 캔버스. 내부의 컨탠츠들의 크기는 조절되지 않으며, 단순히 캔버스 사이즈만 조절되는 것엔 주의.
    /// </summary>
    public class Rect_GoogleSmartBannerAdapt : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;

        public RectTransform CanvasRect;
        public bool isTop;
        private void Awake()
        {
            _eventManager.AdmobSmartBannerSizeChanged += OnAdmobSmartBannerSizeChanged;
        }

        private void OnAdmobSmartBannerSizeChanged(float adHeight, float gap)
        {
            if (isTop)
            {
                UnityRectHelper.SetRectTop(CanvasRect, adHeight+gap);
            }
            else
            {
                UnityRectHelper.SetRectBottom(CanvasRect, adHeight+gap);
            }
      
        }

    }

}
