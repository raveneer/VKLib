using System;

namespace VKLib.NativeExtension
{
    public static class StringExtensions
    {
        /// <summary>
        ///     A 와 B 사이의 첫번째 문자를 가져온다. 첫번째 a와 b만 판정한다.
        ///     ex  "[12](34)" =>  start '[' end ']' return 12
        /// </summary>
        public static string GetBetween(this string str, char start, char end)
        {
            return str.Substring(str.IndexOf(start) + 1, str.IndexOf(end) - str.IndexOf(start) - 1);
        }

        /// <summary>
        ///     A 와 B 사이의 첫번째 문자를 가져온다. 첫번째 a와 b만 판정한다.
        ///     ex  "[12](34)" =>  start '[' end ']' return 12
        /// </summary>
        public static string GetBetween(this string str, string start, string end)
        {
            var pFrom = str.IndexOf(start) + start.Length;
            var pTo = str.LastIndexOf(end);

            return str.Substring(pFrom, pTo - pFrom);
        }

        public static (bool, string) IsValidNumberString(this string amountString)
        {
            if (string.IsNullOrEmpty(amountString)) return (false, "error: amount needed");
            if (!int.TryParse(amountString, out var amount)) return (false, $"{amountString} is not valid number");
            return (true, "");
        }

        public static T ToEnum<T>(this string str)
        {
            return (T) Enum.Parse(typeof(T), str);
        }

        public static bool IsContainOnlyEngKorNum(this string str)
        {
            throw new NotFiniteNumberException();
        }
    }

}