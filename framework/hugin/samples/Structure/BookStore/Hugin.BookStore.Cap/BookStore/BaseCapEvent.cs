using System;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Hugin.BookStore
{
    public abstract class BaseCapEvent : ITransientDependency
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

        protected ICapPublisher CapPublisher => LazyGetRequiredService(ref _capPublisher);
        private ICapPublisher _capPublisher;

        protected BaseCapEvent()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }
    }
}





