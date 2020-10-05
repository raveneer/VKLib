using System.Collections.Generic;

namespace VKLib.NativeExtension
{
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