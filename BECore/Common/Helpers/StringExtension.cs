using System; 
using System.Text;
using System.Text.RegularExpressions; 

namespace Common.Helpers
{
    public static class StringExtension
    {
        public static string Left(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static bool IsStringValid(this string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length < maxLength)
            {
                return true;
            }

            return false;
        }

        public static string RemoveHTML(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.Replace(str, "<.*?>", String.Empty);
            }
            else
                return string.Empty;
        }

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSignForVietnameseString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        public static string ConvertToUnSign(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2.Trim().ToLower();
        }

        public static string CheckNull(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            return input.Trim();
        }

        public static string Escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                // These characters are part of the query syntax and must be escaped
                if (c == '\\' || c == '+' || c == '-' || c == '!' || c == '(' || c == ')' || c == ':'
                  || c == '^' || c == '[' || c == ']' || c == '\"' || c == '{' || c == '}' || c == '~'
                  || c == '*' || c == '?' || c == '|' || c == '&' || c == '/')
                {
                    sb.Append('\\');
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
        public static string ToCamelCase(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;
            return Char.ToLowerInvariant(source[0]) + source.Substring(1);
        }

        public static string ToWildcardSearchValue(this string source)
        {
            return $"*{source}*";
        }

    }
}
