using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class IListExtensions
    {
        public static T Random<T>(this IList<T> list, Random random, int start = 0)
        {
            return list[list.RandomIndex(random)];
        }

        public static int RandomIndex<T>(this IList<T> list, Random random, int start = 0)
        {
            return random.Range(start, list.Count);
        }

        public static T TakeOut<T>(this IList<T> list, int index)
        {
            var taken = list[index];
            list.RemoveAt(index);
            return taken;
        }

        public static void SetAllValue<T>(this IList<T> list, T value)
        {
            for (var i = 0; i < list.Count; i++) list[i] = value;
        }
    }

    public static class IEnumerableExtension
    {
        /// <summary>
        ///     주의. 이 함수는 Distinct 를 할때 메모리를 사용한다.
        ///     이중 콜렉션은 체크하지 못함에 주의.
        ///     https://stackoverflow.com/questions/18303897/test-if-all-values-in-a-list-are-unique
        /// </summary>
        public static bool IsUniqueAll<T>(this IEnumerable<T> collection)
        {
            return collection.Distinct().Count() == collection.Count();
        }
    }

    public static class IDictionaryExtension
    {
        public static void Put<Tkey, Tvalue>(this IDictionary<Tkey, Tvalue> dic, Tkey key, Tvalue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
    }
}