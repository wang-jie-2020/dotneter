using System;
using System.ComponentModel;
using System.Reflection;

namespace Hugin.Sms.ChinaMobile
{
    internal class EnumHelper
    {
        public static string GetEnumDesc<T>(string name)
        {
            Type enumType = typeof(T);
            DescriptionAttribute attr = null;
            if (name != null)
            {
                FieldInfo fieldInfo = enumType.GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                if (fieldInfo != null)
                {
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }

            if (attr != null && !string.IsNullOrEmpty(attr.Description))
                return attr.Description;
            else
                return string.Empty;
        }
    }
}
