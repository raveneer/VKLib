using System.Collections.Generic;
using VKLib.Native;
using Zenject;
using UnityEngine.Analytics;

namespace VKLib.Network
{
    public class UnityAnalyticsManager : IInitializable
    {
        [Inject] private EventManager _eventManager;
        public void Initialize()
        {
            _eventManager.SendUnityAnalyticsCustomEvent += OnSendUnityAnalyticsCustomEvent;
        }

        private void OnSendUnityAnalyticsCustomEvent(string eventName, string key, object value)
        {
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