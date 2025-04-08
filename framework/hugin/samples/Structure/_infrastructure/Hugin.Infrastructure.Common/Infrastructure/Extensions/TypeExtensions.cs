using System;

namespace Hugin.Infrastructure.Extensions
{
    /// <summary>
    /// 类型扩展方法类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断指定类型是否为数值类型
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否是数值类型</returns>
        public static bool IsNumeric(this Type type)
        {
            return type == typeof(byte)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(sbyte)
                || type == typeof(ushort)
                || type == typeof(uint)
                || type == typeof(ulong)
                || type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(float);
        }
    }
}
