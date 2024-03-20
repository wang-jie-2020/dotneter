namespace AESC.Sample.Order.Permissions
{
    public class OrderPermissions
    {
   

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderPermissions));
        }
    }
}