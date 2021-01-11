using UnityEngine;

namespace VKLib.Unity
{
    /// <summary>
    ///    게임을 켜두면 폰이 꺼지지 않게 됨.
    /// </summary>
    public class DontPauseGameByAndroidTimeout : MonoBehaviour
    {
        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}