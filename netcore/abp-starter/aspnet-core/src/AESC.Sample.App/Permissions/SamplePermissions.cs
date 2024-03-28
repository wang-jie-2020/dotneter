namespace AESC.Sample.Permissions
{
    public class SamplePermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SamplePermissions));
        }

        public const string GroupName = "Sample";

        public class Order
        {
            public const string Default = "Order";
            public const string Create = $"{Default}.Create";
            public const string Update = $"{Default}.Update";
            public const string Delete = $"{Default}.Delete";
        }

        public class OrderCustomer
        {
            public const string Default = "OrderCustomer";
            public const string Create = $"{Default}.Create";
            public const string Update = $"{Default}.Update";
            public const string Delete = $"{Default}.Delete";
        }
    }
}