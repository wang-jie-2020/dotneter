using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Hugin.Infrastructure.Helpers
{
    public static class EnumHelper
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

            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }

            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }

            return string.Empty;
        }

        /// <summary>
        /// 返回枚举值的描述信息。
        /// </summary>
        /// <param name="name">要获取描述信息的枚举值。</param>
        /// <returns>枚举值的描述信息。</returns>
        public static string GetEnumDesc<T>(string name)
        {
            Type enumType = typeof(T);
            DescriptionAttribute attr = null;
            if (name != null)
            {
                FieldInfo fieldInfo = enumType.GetField(name, BindingFlags.Public | BindingFlags.Static);
                if (fieldInfo != null)
                {
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }

            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }

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

            FieldInfo fieldInfo = enumType.GetField(e.ToString());
            if (fieldInfo != null)
            {
                attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            }

            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }

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
            throw new ArgumentException($"{num} 未能找到对应的枚举.");
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

        /// <summary>
        /// 获取枚举值列表，并转化为键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isHasAll">是否包含“全部”</param> 
        /// <returns></returns>
        public static List<EnumKeyValue> EnumToList<T>(bool isHasAll = false)
        {
            List<EnumKeyValue> list = new List<EnumKeyValue>();

            if (isHasAll)
            {
                list.Add(new EnumKeyValue() { Key = 0, Desc = "全部" });
            }

            foreach (int item in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), item);
                string desc = "";

                FieldInfo fieldInfo = typeof(T).GetField(name);
                if (fieldInfo != null)
                {
                    var attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null && !string.IsNullOrEmpty(attr.Description))
                    {
                        desc = attr.Description;
                    }
                }

                list.Add(new EnumKeyValue
                {
                    Key = item,
                    Name = name,
                    Desc = desc

                });
            }
            return list;
        }
    }

    /// <summary>
    /// 枚举键值对
    /// </summary>
    public class EnumKeyValue
    {
        public int Key { get; set; }

        public string Desc { get; set; }

        public string Name { get; set; }
    }
}
