using Volo.Abp.Modularity;

namespace Demo
{
    /*
     * 一个模块的最低限度是引用abp.core
     *      集成内容包括Host、Ioc、Configuration、Logger
     */
    public class HelloModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
