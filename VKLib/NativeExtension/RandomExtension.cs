using System;

namespace VKLib.NativeExtension
{
    public static class RandomExtension
    {
        public static int Range(this Random random, int min, int max)
        {
            if (min == max || min > max)
            {
                throw new Exception("min max same or flipped");
            }
            return random.Next(min, max);
        }

        public static double Range(this Random random, double min, double max)
        {
            if (Math.Abs(min - max) < double.Epsilon || min > max)
            {
                throw new Exception("min max same or flipped");
            }
            return random.NextDouble() * (max - min) + min;
        }
    }

}