using System.Reflection;
using Hugin.BookStore.Localization;
using Volo.Abp.Localization;

namespace Hugin.BookStore.Permissions
{
    public class BookStorePermissionDefinitionProvider : Hugin.Application.Permissions.PermissionDefinitionProvider
    {
        protected override LocalizableString L(string name)
        {
            return LocalizableString.Create<BookStoreResource>("Permission:" + name);
        }

        protected override string Root => BookStoreConsts.Name;

        protected override Assembly[] Assemblies => new[] { typeof(BookStorePermissionDefinitionProvider).Assembly };

        public override void Define(Volo.Abp.Authorization.Permissions.IPermissionDefinitionContext context)
        {
            base.Define(context);
        }
    }
}