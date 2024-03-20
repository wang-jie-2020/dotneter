using AESC.Sample.Order.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AESC.Sample.Order
{
    public abstract class OrderController : AbpController
    {
        protected OrderController()
        {
            LocalizationResource = typeof(OrderResource);
        }
    }
}
