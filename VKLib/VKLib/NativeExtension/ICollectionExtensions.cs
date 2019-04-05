using System.Collections.Generic;

namespace VKLib.NativeExtension
{
    public static class ICollectionExtensions
    {
        public static bool Empty<T>(this ICollection<T> collection)
        {
            return collection.Count <= 0;
        }
    }

}