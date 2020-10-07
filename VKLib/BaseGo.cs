using UnityEngine;
using Zenject;

namespace VKLib
{
    /// <summary>
    ///     업데이트 대신 Tick을 사용하여 일시중지 등의 기능을 가지는 Go. 많은 게임오브젝트들의 기본이 된다.
    ///     이 클래스를 상속받으면  goFactory를 써야 함. (인젝션 때문)
    /// </summary>
    public abstract class BaseGo : MonoBehaviour, IGoTickable
    {
        [Inject] private EventManager _eventManager;
        private bool _isDestroying; //ondestroy 로 늦을 때를 위해.

        protected virtual void Start()
        {
            //TDebug.Log( $"{this.GetType().Name} start!");
            _eventManager.GoTick += OnGoTick;
        }

        //주의! OnDestroy에서 처리하면 늦는 경우가 있다. 그럴때는 ReadyToDestroy 를 쓸 것
        protected virtual void OnDestroy()
        {
            _eventManager.GoTick -= OnGoTick;
            //Debug.Log($"{this.GetType().Name} released go tick");
        }

        public virtual void OnGoTick(float deltaTimeSec)
        {
            if (_isDestroying)
            {
                return;
            }

            //TDebug.Log($"{this.GetType().Name} on go tick!");
            GoTick(deltaTimeSec);
        }

        public virtual void ReadyToDestroy()
        {
            _isDestroying = true;
            //Debug.Log($"{this.GetType().Name} ReadyToDestroy");
        }

        /// <summary>
        ///     여기서 update를 구현할 것.
        /// </summary>
        protected abstract void GoTick(float deltaTimeSec);
    }
}