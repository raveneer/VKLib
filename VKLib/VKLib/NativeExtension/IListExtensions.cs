using System;
using System.Collections.Generic;

namespace VKLib.NativeExtension
{
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