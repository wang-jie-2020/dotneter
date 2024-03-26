namespace AESC.Starter.Permissions
{
    public static class StarterPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(StarterPermissions));
        }
    }
}