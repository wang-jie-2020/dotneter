using System;
using System.Text.RegularExpressions;
using AESC.Shared.Localization;

namespace AESC.Sample.Permissions
{
    public class SamplePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(SamplePermissions.GroupName, L(Ps(SamplePermissions.GroupName)));

            //标准格式
            var order = group.AddPermission(SamplePermissions.Order.Default, L(Ps(SamplePermissions.Order.Default)));
            order.AddChild(SamplePermissions.Order.Create, L(CREATE));
            order.AddChild(SamplePermissions.Order.Update, L(UPDATE));
            order.AddChild(SamplePermissions.Order.Delete, L(DELETE));

            var orderCustomer = group.AddPermission(SamplePermissions.OrderCustomer.Default, L(Ps(SamplePermissions.OrderCustomer.Default)));
            orderCustomer.AddChild(SamplePermissions.OrderCustomer.Create, L(CREATE));
            orderCustomer.AddChild(SamplePermissions.OrderCustomer.Update, L(UPDATE));
            orderCustomer.AddChild(SamplePermissions.OrderCustomer.Delete, L(DELETE));

            //约定格式 模块->模块.数据->模块.数据.动作
            {
                var bookStore = group.AddPermission(BookStorePermissions.Default, L(Ps(BookStorePermissions.Default)));

                var bookStorePermissionList = BookStorePermissions.GetAll();
                if (bookStorePermissionList.Length <= 1)
                {
                    return;
                }

                PermissionDefinition parent = null;
                for (var i = 1; i < bookStorePermissionList.Length; i++)
                {
                    if (parent == null || !bookStorePermissionList[i].StartsWith(parent.Name + "."))
                    {
                        parent = bookStore.AddChild(bookStorePermissionList[i], L(Ps(bookStorePermissionList[i])));
                        continue;
                    }

                    parent.AddChild(bookStorePermissionList[i],
                        L(Ps(bookStorePermissionList[i].Substring(bookStorePermissionList[i].LastIndexOf('.') + 1))));

                }

                var finder = bookStore.Children.FirstOrDefault(o => o.Name == BookStorePermissions.BookPermissions.Default);
                finder?.AddChild($"{BookStorePermissions.BookPermissions.Default}.Stop", L(Ps("Stop")));
            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SharedLocalizationResource>(name);
        }

        private static string Ps(string name)
        {
            return $"Permission:{name}";
        }

        private const string CREATE = "Permission:Create";
        private const string UPDATE = "Permission:Update";
        private const string DELETE = "Permission:Delete";
    }
}