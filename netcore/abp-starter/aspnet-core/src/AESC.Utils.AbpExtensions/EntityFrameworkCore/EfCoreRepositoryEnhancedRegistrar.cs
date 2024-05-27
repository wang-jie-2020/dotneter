using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace AESC.Utils.AbpExtensions.EntityFrameworkCore
{
    public class EfCoreRepositoryEnhancedRegistrar : EfCoreRepositoryRegistrar
    {
        public EfCoreRepositoryEnhancedRegistrar([NotNull] AbpDbContextRegistrationOptions options) : base(options)
        {
        }

        protected override void RegisterDefaultRepository(Type entityType)
        {
            // Options.Services.AddDefaultRepository(
            //     entityType,
            //     GetDefaultRepositoryImplementationType(entityType)
            // );
            
            Options.Services.AddDefaultRepository(
                entityType,
                GetDefaultRepositoryImplementationType(entityType),
                true
            );
        }
    }
}