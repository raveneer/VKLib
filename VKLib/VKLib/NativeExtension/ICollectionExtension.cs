using System.Collections.Generic;

namespace VKLib.NativeExtension
{
    public static class ICollectionExtension
    {
        /// <summary>
        /// 콜렉션이 짝수개 원소인지 체크한다. count 를 쓰므로 빠름.
        /// </summary>
        public static bool IsEvenElemCount<T>(this ICollection<T> collection)
        {
            return collection.Count % 2 == 0;
        }
    }
}