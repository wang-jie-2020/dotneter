namespace Hugin.BookStore
{
    public static class BookStoreConsts
    {
        public const string Name = "BookStore";

        public const string NameLower = "bookStore";

        public static class DbProperties
        {
            public const string DbSchema = null;

            public const string DbTablePrefix = Name + "_";

            public const string ConnectionStringName = Name;
        }

        public static class EntityLengths
        {
            public const int Tiny = 50;

            public const int Default = 100;

            public const int More = 500;

            public const int Large = 2000;
        }
    }
}
