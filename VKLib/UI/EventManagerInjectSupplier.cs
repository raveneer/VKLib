using UnityEngine;
using VKLib.Native;
using Zenject;

namespace VKLib.VKLib.UI
{
    /// <summary>
    /// 이벤트매니저를 부득이하게 인젝션 받지 못하는 클래스들은, 이 모노가 씬에 존재한다면, 찾아서 받아갈 수가 있다.
    /// 씬에 존재하여야만 한다.
    /// </summary>
    public class EventManagerInjectSupplier : MonoBehaviour
    {
        [Inject] private EventManager _eventManager;

        public EventManager EventManagerRef => _eventManager;

    }
}