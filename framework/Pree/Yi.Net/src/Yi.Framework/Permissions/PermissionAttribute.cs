namespace Yi.Framework.Permissions;

[AttributeUsage(AttributeTargets.Method)]
public class PermissionAttribute : Attribute
{
    public PermissionAttribute(string code)
    {
        Code = code;
    }

    internal string Code { get; set; }
}