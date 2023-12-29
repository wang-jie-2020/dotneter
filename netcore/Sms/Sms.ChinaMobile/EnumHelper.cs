using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Sms.ChinaMobile
{
    public class EnumHelper
    {
        /// <summary>
        /// 返回枚举值的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举值。</param>
        /// <returns>枚举值的描述信息。</returns>
        public static string GetEnumDesc<T>(int value)
        {
            Type enumType = typeof(T);
            DescriptionAttribute attr = null;

            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }

            // 返回结果
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
                return attr.Description;
            else
                return string.Empty;
        }

        /// <summary>
        /// 返回枚举值的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举值。</param>
        /// <returns>枚举值的描述信息。</returns>
        public static string GetEnumDesc<T>(string name)
        {
            Type enumType = typeof(T);
            DescriptionAttribute attr = null;
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name, BindingFlags.Public  | BindingFlags.Static | BindingFlags.IgnoreCase );
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }

            // 返回结果
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
                return attr.Description;
            else
                return string.Empty;
        }

        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="e">要获取描述信息的枚举项。</param>
        /// <returns>枚举项的描述信息。</returns>
        public static string GetEnumDesc(Enum e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            Type enumType = e.GetType();
            DescriptionAttribute attr = null;

            // 获取枚举字段。
            FieldInfo fieldInfo = enumType.GetField(e.ToString());
            if (fieldInfo != null)
            {
                // 获取描述的属性。
                attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            }

            // 返回结果
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
                return attr.Description;
            else
                return string.Empty;
        }

        /// <summary>
        /// int获取枚举
        /// </summary>
        /// <typeparam name="T">枚举集合</typeparam>
        /// <param name="num"></param>
        /// <returns></returns>
        public static T NumToEnum<T>(int num)
        {
            foreach (var myCode in Enum.GetValues(typeof(T)))
            {
                if ((int)myCode == num)
                {
                    return (T)myCode;
                }
            }
            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", num), "Description");
        }

        public static string GetEnumName<T>(int value)
        {
            Type enumType = typeof(T);
            return Enum.GetName(enumType, value);
        }

        public static string GetEnumName(Type enumType, int value)
        {
            return Enum.GetName(enumType, value);
        }
    }
}
