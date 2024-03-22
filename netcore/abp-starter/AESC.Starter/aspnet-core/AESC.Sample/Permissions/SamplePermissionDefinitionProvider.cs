using AESC.Sample.Localization;

namespace AESC.Sample.Permissions
{
    public class SamplePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SampleLocalizationResource>(name);
        }
    }
}