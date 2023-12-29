namespace Hugin.BookStore.Permissions
{
    public partial class BookStorePermissions
    {
        public static class Author
        {
            public const string Default = Group.Default + ".Author";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}