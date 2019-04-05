using System.Collections.Generic;

namespace VKLib.NativeExtension
{
    public static class ArrayExtensions
    {
        public static void Add<T>(this ICollection<T>[,] array, int index1,
                                  int index2, T value)
        {
            ICollection<T> existingCollection = array[index1, index2];
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
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = value;
                }
            }
        }

        public static void SetAllValue<T>(this T[] array, T value)
        {
            for (int x = 0; x < array.Length; x++)
            {
                array[x] = value;
            }
        }

    }
}