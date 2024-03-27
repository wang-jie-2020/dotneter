using Yitter.IdGenerator;

namespace AESC.Utils.Common
{
    /// <summary>
    ///  单机Id,配合EF的CodeFirst使用按需要[DatabaseGenerated(DatabaseGeneratedOption.None)]
    /// </summary>
    public static class IdGen
    {
        static IdGen()
        {
            var options = new IdGeneratorOptions(0);
            YitIdHelper.SetIdGenerator(options);
        }

        public static long Next()
        {
            return YitIdHelper.NextId();
        }
    }
}
