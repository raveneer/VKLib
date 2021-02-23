using System;
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

        public static void Push<Tkey>(this IDictionary<Tkey,int> dic, Tkey key, int value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] += value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        
        public static void Push<Tkey>(this IDictionary<Tkey,float> dic, Tkey key, float value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] += value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        
        public static void Push<Tkey>(this IDictionary<Tkey,double> dic, Tkey key, double value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] += value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
     
    }
}