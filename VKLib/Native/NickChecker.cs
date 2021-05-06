using System.Collections.Generic;

namespace VKLib.Native
{
    public class NickChecker
    {
        private static readonly List<string> BadWords = new List<string>()
        {
            "sex", "Sex", "SEX", "섹스", "섹쓰", "쎅쓰", "쎆쓰", "쎅스"
        };

        public static bool IsNickContainsOnlyGoodWords(string nick, out string foundBadWord)
        {
            foundBadWord = "";
            foreach (var bad in BadWords)
            {
                if (nick.Contains(bad))
                {
                    foundBadWord = bad;
                    return false;
                }
            }

            return true;
        }

        public static bool IsValidWordForNick(string nick, out string reason)
        {
            reason = "";
            if (string.IsNullOrEmpty(nick) || nick.Length < 2 || nick.Length > 10)
            {
                reason = "닉네임을 2-10글자로 지어주세요";
                return false;
            }

            if (nick.Contains(" "))
            {
                reason = "스페이스와 띄어쓰기는 닉에 사용할 수 없습니다.";
                return false;
            }

            if (!nick.IsContainOnlyEngKorNum())
            {
                reason = "한글 영어 숫자만 사용가능합니다.";
                return false;
            }

            if (!IsNickContainsOnlyGoodWords(nick, out var foundBadWord))
            {
                reason = $"{foundBadWord}은(는) 닉으로 사용할 수 없는 단어입니다";
                return false;
            }

            return true;
        }
    }
}