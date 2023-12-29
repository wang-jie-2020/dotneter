using System;

namespace Hugin.Application.Filters
{
    /// <summary>
    /// 标记查询字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class QueryFilterFieldAttribute : Attribute
    {
        public QueryFilterFieldAttribute()
        {
        }
    }
}