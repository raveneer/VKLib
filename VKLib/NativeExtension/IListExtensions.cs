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

        public static bool TryGetValue<T>(this IList<T> list, int index, out T found)
        {
            //index1일때, count는2는 되어야 함. 
            if (list.Count <= index)
            {
                found = default;
                return false;
            }
            found = list[index];
            return true;
        }


        public static T Random<T>(this IList<T> list, int start = 0)
        {
            return list[list.RandomIndex(start)];
        }

        public static int RandomIndex<T>(this IList<T> list, int start = 0)
        {
            return UnityEngine.Random.Range(start, list.Count);
        }
        
        /// <summary>
        /// return nextelem or first elem. if one elem in list, return that.
        /// </summary>
        public static T GetNextElem<T>(this IList<T> list, T current)
        {
            if (current == null)
            {
                return list[0];
            }

            if (list.Count == 1)
            {
                return current;
            }

            if (!list.Contains(current))
            {
                throw new ArgumentException($"{current} not in list!");
            }
            int indexOfCurrent = list.IndexOf(current);
            if (indexOfCurrent == list.Count - 1)
            {
                return list[0];
            }
            else
            {
                return list[indexOfCurrent + 1];
            }
            
        }
    }

}