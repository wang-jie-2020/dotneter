namespace AESC.Sample.Order.Permissions
{
    public class OrderPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
           
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OrderResource>(name);
        }
    }
}