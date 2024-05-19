using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DR.Utilities.Extensions
{
    public static class StringExtension
    {
        public enum StringMatchType
        {
            Exactly,
            ExactlyIgnoreCase,
            Contains,
            ContainsIgnoreCase
        }
        
        public static bool IsMatchWith(this string @this, string comparedString, StringMatchType matchType = StringMatchType.Exactly)
        {
            switch (matchType)
            {
                default:
                case StringMatchType.Exactly:
                    return string.Equals(@this, comparedString, System.StringComparison.Ordinal);
                case StringMatchType.ExactlyIgnoreCase:
                    return string.Equals(@this, comparedString, System.StringComparison.OrdinalIgnoreCase);
                case StringMatchType.Contains:
                    return @this.Contains(comparedString);
                case StringMatchType.ContainsIgnoreCase:
                    return @this.Contains(comparedString, true);
            }
        }
        
        public static bool Contains(this string @this, string comparedString, bool ignoreCase)
        {
            if (ignoreCase)
            {
                comparedString = comparedString.ToLower();
                @this = @this.ToLower();
            }
            return @this.Contains(comparedString);
        }
        
        public static string SplitCamelCase(this string camelCaseString)
        {
            if (string.IsNullOrEmpty(camelCaseString))
                return camelCaseString;

            string camelCase = Regex.Replace(Regex.Replace(camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                @"(\p{Ll})(\P{Ll})", "$1 $2");
            string firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (camelCaseString.Length > 1)
            {
                string rest = camelCase.Substring(1);

                return firstLetter + rest;
            }

            return firstLetter;
        }

        public static string CapitalizeFirstChar(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            char firstChar = input[0];

            if (char.IsUpper(firstChar))
                return input;

            var chars = input.ToCharArray();
            chars[0] = char.ToUpper(firstChar);
            return new string(chars);
        }

        public static float TryParseToFloat(this string @this)
        {
            bool canParse = float.TryParse(@this, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result);
            if (canParse)
                return result;
            else
                return 0;
        }
    }
}