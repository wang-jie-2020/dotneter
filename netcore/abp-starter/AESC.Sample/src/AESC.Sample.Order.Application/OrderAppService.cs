namespace AESC.Sample.Order
{
    public abstract class OrderAppService : ApplicationService
    {
        protected OrderAppService()
        {
            LocalizationResource = typeof(OrderResource);
            ObjectMapperContext = typeof(OrderApplicationModule);
        }
    }
}
