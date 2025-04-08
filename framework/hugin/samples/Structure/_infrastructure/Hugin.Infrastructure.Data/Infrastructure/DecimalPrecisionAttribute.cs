using System;

namespace Hugin.Infrastructure
{
    /// <summary>
    /// Decimal精度
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// 精度,默认18
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// 小数位数，默认2
        /// </summary>
        public byte Scale { get; set; }

        public DecimalPrecisionAttribute()
        {
            Precision = 18;
            Scale = 2;
        }

        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }
    }
}
