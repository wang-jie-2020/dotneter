namespace AESC.Sample.Order
{
    public static class OrderDbProperties
    {
        public static string DbTablePrefix { get; set; } = "";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Order";
    }
}
