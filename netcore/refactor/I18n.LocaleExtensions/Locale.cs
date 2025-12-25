using System;
using System.Globalization;
using System.Threading;

namespace I18n.LocaleExtensions
{
    public class Locale
    {
        public string Language { get; }
        
        public TimeZoneInfo TimeZone { get; }
        
        public CultureInfo Culture { get; }

        public Locale(string language, TimeZoneInfo timeZone, CultureInfo culture)
        {
            Language = language;
            TimeZone = timeZone;
            Culture = culture;
        }
        
        public static Locale CN = new Locale("zh-CN", TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai"), CultureInfo.GetCultureInfo("zh-CN"));
        public static Locale US = new Locale("en-US", TimeZoneInfo.FindSystemTimeZoneById("America/New_York"), CultureInfo.GetCultureInfo("en-US"));
        public static Locale JP = new Locale("ja-JP", TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"), CultureInfo.GetCultureInfo("ja-JP"));
        public static Locale FR = new Locale("fr_FR", TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris"), CultureInfo.GetCultureInfo("fr_FR"));
        
        private static AsyncLocal<Locale> _locale = new AsyncLocal<Locale>();
    }
}