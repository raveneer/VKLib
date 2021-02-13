using System.Collections.Generic;
using UnityEngine;
using VKLib.Native;
using Zenject;
using UnityEngine.Analytics;

namespace VKLib.Network
{
    /// <summary>
    /// todo : 개발자 계정이나 에디터에서는 보내지 말아야 (집계되면 안되니까) 할 것. 일단은 에디터 막기.
    /// </summary>
    public class UnityAnalyticsManager : IInitializable
    {
        [Inject] private EventManager _eventManager;
        public void Initialize()
        {
            _eventManager.SendUnityAnalyticsCustomEvent += OnSendUnityAnalyticsCustomEvent;
        }

        private void OnSendUnityAnalyticsCustomEvent(string eventName, string key, object value)
        {
            if (Application.isEditor)
            {
                TDebug.Log($"{eventName} {key}  {value.ToString()} 아날리틱스 발동하였으나, 에디터 이므로 패스.");
            }

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