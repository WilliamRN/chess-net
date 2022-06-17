using System.Text;

namespace ChessNet.Data.Utils
{
    public static class StringUtils
    {
        private static readonly string _characters = "abcdefghijklmnopqrstuvwxyz";
        private static string _charactersAsUpperCase => _characters.ToUpper();
        private static readonly string _numbers = "0123456789";

        public static string GetNumbersOnly(this string input)
        {
            string result = "";

            var cleanedString = input
                .Slugify(isRemoving: true);

            foreach (var c in cleanedString)
            {
                if (_numbers.Contains(c))
                    result += c;
            }

            return result;
        }

        public static string GetLettersOnly(this string input)
        {
            string result = "";

            var cleanedString = input
                .Slugify(isRemoving: true);

            foreach (var c in cleanedString)
            {
                if (_characters.Contains(c) ||
                    _charactersAsUpperCase.Contains(c))
                    result += c;
            }

            return result;
        }

        public static string RemoveSpecialCharacters(this string input, char[] ignoreList = null)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            List<char> ignoreCharList = new List<char>();

            if (ignoreList != null)
            {
                ignoreCharList.AddRange(ignoreList);
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9')
                    || (c >= 'A' && c <= 'Z')
                    || (c >= 'a' && c <= 'z')
                    || (ignoreCharList.Contains(c)))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string Slugify(this string data, bool isAcceptingExtended = false, bool isRemoving = false, string keepChars = "")
        {
            if (string.IsNullOrEmpty(data))
                return data;

            string result = data;

            string from = "ãàáäâẽèéëêìíïîõòóöôùúüûñçÃÀÁÄÂẼÈÉËÊÌÍÏÎÕÒÓÖÔÙÚÜÛÑÇ";
            string to = "aaaaaeeeeeiiiiooooouuuuncAAAAAEEEEEIIIIOOOOOUUUUNC";
            string valid = _characters + _numbers + _charactersAsUpperCase;
            string validExtended = valid + " -=_+|!@#$%&*()[]'\"/,.:;?\n\r";

            for (int i = 0, l = from.Length; i < l; i++)
            {
                result = result.Replace(from.Substring(i, 1), to.Substring(i, 1));
            }

            // remove invalid chars
            foreach (char c in result)
            {
                if (isAcceptingExtended ? !validExtended.Contains(c) : !valid.Contains(c))
                {
                    if (!keepChars.Contains(c))
                    {
                        result = isRemoving ? result.Replace(c.ToString(), string.Empty) : result.Replace(c, '-');
                    }
                }
            }

            return result;
        }
    }
}