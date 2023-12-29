namespace Hugin.Platform
{
    public static partial class PlatformConsts
    {
        public const string Name = "Platform";

        public const string NameLower = "platform";

        public static class DbProperties
        {
            public static string DbSchema { get; set; } = null;

            public static string DbTablePrefix { get; set; } = PlatformConsts.NameLower + "_";

            public const string ConnectionStringName = PlatformConsts.Name;
        }

        public static class Lengths
        {
            public const int Tiny = 50;

            public const int Default = 100;

            public const int More = 500;

            public const int Large = 2000;
        }
    }
}
