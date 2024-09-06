using Volo.Abp.Domain.Values;

namespace Yi.Infra.Rbac.Entities.ValueObjects
{
    public class EncryPasswordValueObject : ValueObject
    {
        public EncryPasswordValueObject() { }
        public EncryPasswordValueObject(string password) { this.Password = password; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 加密盐值
        /// </summary>
        public string Salt { get; set; } = string.Empty;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Password;
            yield return Salt;
        }
    }
}
