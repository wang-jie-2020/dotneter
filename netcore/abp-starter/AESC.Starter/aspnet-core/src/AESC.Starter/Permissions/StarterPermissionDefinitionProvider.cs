namespace AESC.Starter.Permissions
{
    public class StarterPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
           

       
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<StarterResource>(name);
        }
    }
}