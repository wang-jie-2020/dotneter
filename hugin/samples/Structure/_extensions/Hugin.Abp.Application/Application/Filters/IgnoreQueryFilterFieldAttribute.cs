using System;

namespace Hugin.Application.Filters
{
    /// <summary>
    /// 标记忽略查询字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IgnoreQueryFilterFieldAttribute : Attribute
    {
        public IgnoreQueryFilterFieldAttribute()
        {
        }
    }
}