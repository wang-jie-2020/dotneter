namespace Hugin.BookStore.Permissions
{
    public partial class BookStorePermissions
    {
        public static class BookShop
        {
            public const string Default = Group.Default + ".BookShop";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Stop = Default + ".Stop";
        }
    }
}