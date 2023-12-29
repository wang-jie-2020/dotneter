using System;

namespace Generator.Extensions
{
    public static class StringExtensions
    {
        public static string TrimStart(this string originalString, string trimString)
        {
            var index = originalString.IndexOf(trimString, StringComparison.OrdinalIgnoreCase);
            if (index == -1)
            {
                return originalString;
            }

            return originalString.Substring(trimString.Length).TrimStart(trimString);
        }
    }
}