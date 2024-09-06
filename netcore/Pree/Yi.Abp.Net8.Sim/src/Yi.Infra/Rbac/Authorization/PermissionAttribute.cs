namespace Yi.Infra.Rbac.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]

    public class PermissionAttribute : Attribute
    {
        internal string Code { get; set; }

        public PermissionAttribute(string code)
        {
            Code = code;
        }


    }
}
