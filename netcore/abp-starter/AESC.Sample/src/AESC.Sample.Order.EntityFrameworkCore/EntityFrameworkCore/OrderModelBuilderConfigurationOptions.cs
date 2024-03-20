namespace AESC.Sample.Order.EntityFrameworkCore
{
    public class OrderModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrderModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}