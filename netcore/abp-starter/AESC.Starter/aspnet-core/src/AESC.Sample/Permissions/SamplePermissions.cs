namespace AESC.Sample.Permissions
{
    public class SamplePermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SamplePermissions));
        }
    }
}