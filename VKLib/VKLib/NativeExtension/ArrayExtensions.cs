using System;
using System.Collections.Generic;

namespace VKLib.NativeExtension
{
    public static class ArrayExtensions
    {
        /// <summary>
        ///     for 등에서 반복을 위해 lengh -1 등을 하는 것을 간단히 표현.
        ///     빈 배열의 경우에는 에러.
        /// </summary>
        public static int LastIndex<T>(this T[] array)
        {
            if (array.IsEmpty())
            {
                throw new ArgumentException(" array empty!");
            }
            return array.Length - 1;
        }

        /// <summary>
        ///     배열 길이가 0인지 체크. array.Length == 0; 의 축약형
        /// </summary>
        public static bool IsEmpty<T>(this T[] array)
        {
            return array.Length == 0;
        }

        public static void Add<T>(this ICollection<T>[,] array, int index1,
                                  int index2, T value)
        {
            var existingCollection = array[index1, index2];
            if (existingCollection != null)
            {
                existingCollection.Add(value);
                return;
            }

            var set = new HashSet<T>();
            set.Add(value);
            array[index1, index2] = set;
        }

        public static bool Remove<T>(this ICollection<T>[,] array, int index1,
                                     int index2, T value)
        {
            var existingCollection = array[index1, index2];
            if (existingCollection != null && !existingCollection.Empty())
            {
                return existingCollection.Remove(value);
            }
            return false;
        }

        public static void SetAllValue<T>(this T[,] array, T value)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++) array[x, y] = value;
            }
        }

        public static void SetAllValue<T>(this T[] array, T value)
        {
            for (var x = 0; x < array.Length; x++) array[x] = value;
        }

        /// <summary>
        ///     배열이 짝수개 배열인지 체크한다. length 를 쓰므로 빠름.
        /// </summary>
        public static bool IsEvenElemCount<T>(this T[] array)
        {
            return (array.Length) % 2 == 0;
        }
    }
}