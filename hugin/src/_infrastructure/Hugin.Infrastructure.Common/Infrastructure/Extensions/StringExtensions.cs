using System;

namespace Hugin.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool Compare(this string orig, string target)
        {
            return String.Compare(orig, target, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool Compare(this string orig, string target, bool ignoreCase)
        {
            return string.Compare(orig, target, ignoreCase) == 0;
        }

        public static bool IsEmpty(this string str)
        {
            return ((str == null) || (str.ToString().Trim().Length == 0));
        }
    }
}
