using UnityEngine;
using Zenject;

namespace VKLib
{
    /// <summary>
    ///     ������Ʈ ��� Tick�� ����Ͽ� �Ͻ����� ���� ����� ������ Go. ���� ���ӿ�����Ʈ���� �⺻�� �ȴ�.
    ///     �� Ŭ������ ��ӹ�����  goFactory�� ��� ��. (������ ����)
    /// </summary>
    public abstract class BaseGo : MonoBehaviour, IGoTickable
    {
        [Inject] private EventManager _eventManager;
        private bool _isDestroying; //ondestroy �� ���� ���� ����.

        protected virtual void Start()
        {
            //TDebug.Log( $"{this.GetType().Name} start!");
            _eventManager.GoTick += OnGoTick;
        }

        //����! OnDestroy���� ó���ϸ� �ʴ� ��찡 �ִ�. �׷����� ReadyToDestroy �� �� ��
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
        ///     ���⼭ update�� ������ ��.
        /// </summary>
        protected abstract void GoTick(float deltaTimeSec);
    }
}