using Yi.AspNetCore.Data;

namespace Yi.Framework.Core.Entities.ValueObjects;

public class EncryptPasswordValueObject : ValueObject
{
    public EncryptPasswordValueObject()
    {
    }

    public EncryptPasswordValueObject(string password)
    {
        Password = password;
    }

    /// <summary>
    ///     密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     加密盐值
    /// </summary>
    public string Salt { get; set; } = string.Empty;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Password;
        yield return Salt;
    }
}