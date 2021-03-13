using System.Text.RegularExpressions;

namespace VKLib.Native
{
    public static class StringExtention
    {
        public static bool IsContainOnlyEngKorNum(this string str)
        {
            var pattern = @"^[ㄱ-ㅎ|가-힣|ㅏ-ㅣ|a-z|A-Z|0-9|\*]+$";

            return Regex.Match(str, pattern).Success;
        }
    }
}