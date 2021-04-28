using UnityEngine;
using UnityEngine.EventSystems;
using VKLib.Native;
using Zenject;

namespace VKLib.VKLib.UI
{
    /// <summary>
    ///     강제로 버튼을 찾아 누르는 기능. 디버그나 유니티 테스트 등에 사용된다.
    ///     씬에 배치시켜 놓으면 이벤트로 특정 버튼을 누를 수 있게 된다. (테스트에 사용)
    /// </summary>
    public class ForceButtonDownSystem : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;

        private void Awake()
        {
            _eventManager.ForceButtonDown += OnForceButtonDown;
        }

        private void OnForceButtonDown(string buttonGameObjectName)
        {
            var found = GameObject.Find(buttonGameObjectName);
            if (found == null)
            {
                TDebug.Log($"{buttonGameObjectName} 버튼을 찾지 못했습니다.");
            }
            else
            {
                ExecuteEvents.Execute<IPointerClickHandler>(found, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                TDebug.Log($"{buttonGameObjectName} 버튼 눌러짐");
            }
        }
    }
}