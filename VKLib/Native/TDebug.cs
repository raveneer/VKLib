using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace VKLib.Native
{
    /// <summary>
    ///     유니티 의 디버그 기능을 시도하며, 불가시 시스템 디버그 기능을 시도하는 헬퍼 클래스.
    ///     이렇게 함으로서 NCrunch 의 테스트와 UnityTest 양쪽에서 구동할 수 있게 된다.
    ///     hack : 실기에서 로그를 읽으면 공유충돌이 나기에 부득이 메모리 로그를 씀.
    ///     todo : LogError 는 어떻게 여기에 넣을 수 있나?
    /// </summary>
    public static class TDebug
    {
        public static Queue<string> LogQueue = new Queue<string>();

        public static Action<string> OnLog;

        private static readonly int _maxLogQueueSize = 1000;

        public static void AddQueueAtMaxSize(string log)
        {
            OnLog?.Invoke(log);

            if (LogQueue.Count >= _maxLogQueueSize)
            {
                LogQueue.Dequeue();
            }
            LogQueue.Enqueue(log);
        }

        /// <summary>
        /// 기본적인 bool 체크 단정. 추가적인 정보가 부족하므로 직접 사용하기보다는 다른 래퍼를 쓰는 것을 추천.
        /// </summary>
        public static void Assert(bool condition, string log = "")
        {
            if (condition)
            {
                return;
            }

            //hack : 릴리즈 모드에서도 어서트가 작동하기 위해, 로깅한 후 '잡지 않는 예외'를 던진다. 다만 릴리즈 모드에서는 유저가 알 수가 없음이 문제.
            //다행히 유니티 퍼포먼스 리포팅에는 기록된다.
            //todo : 팝업을 보여주고 웹로그로 남기는 쪽이 나을지도?
            try
            {
                UnityEngine.Debug.LogError($"assert error! + {log}");
                AddQueueAtMaxSize($"assert error! + {log}");
                throw new Exception($"assert error! {log}");
            }
            //유닛테스트 동안에는 이걸 사용.
            catch (System.Security.SecurityException)
            {
                //System.Diagnostics.Debug.Assert(condition);
                throw new Exception($"assert error! {log}");
            }
        }

        /// <summary>
        /// 특정 참조가 null이 아님을 단정. 이걸 쓰면 해당 참조의 이름을 보고하므로 유용하다.
        /// 이것은 fatal error 이므로, 로그를 찍을 필요가 없음. 
        /// </summary>
        public static void AssertNotNull([CanBeNull] object obj,  string paramName, [System.Runtime.CompilerServices.CallerMemberName] string caller = "",  string additionalInfo = "")
        {
            if (obj != null) return;
            Log($"{paramName} is null! in {caller}() method");
            throw new ArgumentNullException(paramName, additionalInfo);
        }

        public static void Log(string text)
        {
            try
            {
                UnityEngine.Debug.Log(text);
                AddQueueAtMaxSize(text);
            }
            catch (System.Security.SecurityException)
            {
                System.Diagnostics.Debug.WriteLine(text);
                AddQueueAtMaxSize(text);
            }
        }

        public static void LogWarning(string text)
        {
            try
            {
                UnityEngine.Debug.LogWarning(text);
                AddQueueAtMaxSize(text);
            }
            catch (System.Security.SecurityException)
            {
                System.Diagnostics.Debug.WriteLine($"Warning! : +{text}");
                AddQueueAtMaxSize(text);
            }
        }
    }
}
