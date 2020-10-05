using System;

namespace VKLib.NativeExtension
{
    public static class NumberExtension
    {
        /// <summary>
        ///     값을 일정 범위 안으로 바꾼다.
        ///     반환값을 받아서 사용할 것. 입력값을 바꾸지 않기 때문에.
        /// </summary>
        public static float SetRange(this float current, float a, float b)
        {
            float max;
            float min;

            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            if (current >= max)
            {
                return max;
            }
            if (current <= min)
            {
                return min;
            }

            return current;
        }

        /// <summary>
        ///     값을 일정 범위 안으로 바꾼다.
        ///     반환값을 받아서 사용할 것. 입력값을 바꾸지 않기 때문에.
        /// </summary>
        public static int SetRange(this int current, int a, int b)
        {
            int max;
            int min;

            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            if (current >= max)
            {
                return max;
            }
            if (current <= min)
            {
                return min;
            }

            return current;
        }

        /// <summary>
        ///     최소치와 최대치를 '포함' 한다.
        /// </summary>
        public static bool IsInRange(this int current, int a, int b)
        {
            int max;
            int min;

            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            //PDC.Logger.Log($"max {max}, min {min}, currnt {current}");
            if (current > max)
            {
                return false;
            }
            if (current < min)
            {
                return false;
            }
            return true;
        }

        public static bool IsInRange(this float current, float a, float b)
        {
            float max;
            float min;

            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            //PDC.Logger.Log($"max {max}, min {min}, currnt {current}");
            if (current > max)
            {
                return false;
            }
            if (current < min)
            {
                return false;
            }
            return true;
        }

        public static double ToRadians(this double val)
        {
            return Math.PI / 180 * val;
        }
    }

    public static class DateTimeExtension
    {
        public static float GetTotalSeconds(this DateTime time)
        {
            var dtEnd = new DateTime();
            var elapsed = time - dtEnd;

            return (float) elapsed.TotalSeconds;
        }
    }
}