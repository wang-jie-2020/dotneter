using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Hugin.Platform
{
#if DEBUG
    [Queue("debug")]
#endif
    public abstract class BaseBackgroundJob : ITransientDependency
    {
        public IServiceProvider ServiceProvider { get; set; }

        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }

            return reference;
        }

        protected IClock Clock => LazyGetRequiredService(ref _clock);
        private IClock _clock;

        public IGuidGenerator GuidGenerator { get; set; }

        protected IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);
        private IUnitOfWorkManager _unitOfWorkManager;

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected BaseBackgroundJob()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }
    }
}





