using System.Collections.Generic;
using System.Linq;

namespace VKLib.NativeExtension
{
    public static class IEnumerableExtension
    {
        /// <summary>
        ///     주의. 이 함수는 Distinct 를 할때 메모리를 사용한다.
        ///     이중 콜렉션은 체크하지 못함에 주의.
        ///     https://stackoverflow.com/questions/18303897/test-if-all-values-in-a-list-are-unique
        /// </summary>
        public static bool IsUniqueAll<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Distinct().Count() == enumerable.Count();
        }

        /// <summary>
        /// 콜렉션이 짝수개 원소인지 체크한다. count() 를 쓰므로 느리다는 것에 주의.
        /// </summary>
        public static bool IsEvenElemCount<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() % 2 == 0;
        }

        /// <summary>
        /// !Enumerable.Any() 의 축약형.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}