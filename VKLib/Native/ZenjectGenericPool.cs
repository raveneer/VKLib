using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Zenject;

namespace VKLib.Native
{
    /// <summary>
    ///     젠젝트를 이용하는 제네릭 오브젝트 풀. IPooledObject을 구현하는 객체는 이 풀을 이용하여 타입별로 스폰/디스폰 가능하다.
    /// </summary>
    public class ZenjectGenericPool
    {
        [Inject] private readonly DiContainer _diContainer;
        private readonly ServiceContainer _serviceContainer;

        public ZenjectGenericPool()
        {
            _serviceContainer = new ServiceContainer();
        }

        public void DeSpawn<T>(T item) where T : IPooledObject
        {
            item.OnDeSpawned();
            item.Reset(); //디스폰 후에 불러짐에 주의
            if (!HasList<T>()) MakeList<T>();
            GetList<T>().Add(item);
        }

        public int GetPooledAmount<T>() where T : IPooledObject
        {
            if (HasList<T>()) return GetList<T>().Count;

            return 0;
        }

        public T Spawn<T>() where T : IPooledObject
        {
            T item = default;
            if (GetPooledAmount<T>() > 0)
            {
                item = GetList<T>().First();
                GetList<T>().RemoveAt(0);
            }
            else
            {
                if (_diContainer == null)
                {
                    throw new Exception();
                }
                item = _diContainer.Instantiate<T>();
            }
            item.Reset(); //스폰 전에 불러짐에 주의
            item.OnSpawned();
            return item;
        }

        private List<T> GetList<T>() where T : IPooledObject
        {
            return _serviceContainer.GetService(typeof(List<T>)) as List<T>;
        }

        private bool HasList<T>() where T : IPooledObject
        {
            return _serviceContainer.GetService(typeof(List<T>)) != null;
        }

        private void MakeList<T>() where T : IPooledObject
        {
            if (_serviceContainer.GetService(typeof(List<T>)) == null)
            {
                _serviceContainer.AddService(typeof(List<T>), new List<T>());
            }
            else
            {
                throw new Exception("already have list!");
            }
        }
    }

    public interface IPooledObject
    {
        /// <summary>
        ///     생성되었을 때 호출된다. 참조를 할당하거나 값을 세팅할 것.
        /// </summary>
        void OnSpawned();

        /// <summary>
        ///     풀에 돌아갈 때 호출된다. 이벤트 핸들링 등을 해제할 것.
        /// </summary>
        void OnDeSpawned();

        /// <summary>
        ///     값을 초기화 한다. OnSpawned의 이전, OnDeSpawned의 이후에 1회씩 총 2회 호출된다.
        /// </summary>
        void Reset();
    }
}