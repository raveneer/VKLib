using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using VKLib.Native;
using Zenject;

namespace VKLib.Network
{
    /// <summary>
    ///     todo : 개발자 계정이나 에디터에서는 보내지 말아야 (집계되면 안되니까) 할 것. 일단은 에디터 막기.
    ///     처리량이 많으면 게임이 느려지곤 하기 때문에, 큐잉으로 처리한다. 1틱에 1건씩을 보고한다.
    ///     이걸로도 느리면, 스레드를 써야 할 것이다...
    /// </summary>
    public class UnityAnalyticsManager : IInitializable
    {
        [Inject] private EventManager _eventManager;
        private readonly Queue<(string eventName, string key, object value)> _sendQueue = new Queue<(string, string, object)>();

        public void Initialize()
        {
            _eventManager.SendUnityAnalyticsCustomEvent += OnSendUnityAnalyticsCustomEvent;
            _eventManager.Tick += OnTick;
        }

        private void OnTick(float delta)
        {
            if (_sendQueue.Any())
            {
                SendAnalytics();
            }
        }

        private void OnSendUnityAnalyticsCustomEvent(string eventName, string key, object value)
        {
            if (Application.isEditor)
            {
                TDebug.Log($"{eventName} {key}  {value.ToString()} 아날리틱스 발동하였으나, 에디터 이므로 패스.");
                return;
            }
            else
            {
                _sendQueue.Enqueue((eventName, key, value));
            }
        }

        private void SendAnalytics()
        {
            var elem = _sendQueue.Dequeue();
            var eventName = elem.eventName;
            var key = elem.key;
            var value = elem.value;
            var res = Analytics.CustomEvent(eventName, new Dictionary<string, object>() {{key, value}});
            if (res == AnalyticsResult.Ok)
            {
                TDebug.Log($"unity analytics sent : {eventName}, {key}, {value.ToString()}");
            }
            else
            {
                TDebug.LogWarning($"unity analytics sent fail! code {res} : {eventName}, {key}, {value.ToString()}");
            }
        }
    }
}