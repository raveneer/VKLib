using System;
using System.Collections.Generic;
using System.Linq;

namespace VKLib.NativeExtension
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetIterate<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}