namespace Hugin.BookStore.Permissions
{
    public partial class BookStorePermissions
    {
        public static class Book
        {
            public const string Default = Group.Default + ".Book";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}