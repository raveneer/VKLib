using System;

namespace VKLib.Native
{
    public static class VKMath
    {
        public static float Lerp(float start, float end, float t)
        {
            return start + t * (end - start);
        }

        public static double Lerp(double start, double end, float t)
        {
            return start + t * (end - start);
        }

        public static float Progress(float current, float currentLvStart, float nextLvStart)
        {
            if (nextLvStart == currentLvStart)
            {
                throw new Exception($"nextLvStart must larger than currentLvStart! n:{nextLvStart}, c:{currentLvStart}");
            }

            return (current - currentLvStart) / (nextLvStart - currentLvStart);
        }
    }

}