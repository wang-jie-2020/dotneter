using System;
using System.ComponentModel.DataAnnotations;

namespace Hugin.Application.Validates
{
    /// <summary>
    /// 标注值类型不可为默认值
    /// </summary>
    public class NotDefaultAttribute : ValidationAttribute
    {
        const string DefaultErrorMessage = "{0}字段不能填写无效值";

        public NotDefaultAttribute() : base(DefaultErrorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }

            return true;
        }
    }
}
