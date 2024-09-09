using Volo.Abp.Application;
using Volo.Abp.Application.Dtos;

namespace Yi.Framework.Ddd.Application;

[DependsOn(typeof(AbpDddApplicationModule))]
public class YiFrameworkDddApplicationModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        //分页限制
        LimitedResultRequestDto.DefaultMaxResultCount = 10;
        LimitedResultRequestDto.MaxMaxResultCount = 10000;
    }
}