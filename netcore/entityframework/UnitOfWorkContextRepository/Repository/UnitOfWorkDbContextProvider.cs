using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkContextRepository.Transaction;

namespace UnitOfWorkContextRepository.Repository
{
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _serviceProvider = serviceProvider;
        }

        public async Task<TDbContext> GetDbContextAsync()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                //unitOfWork未开启,则直接从当前域内解析,在abp的思路中,如果unitOfWork未开启是直接作为错误返回的
                return CreateDbContext();
            }

            var key = typeof(TDbContext).FullName;
            var context = (TDbContext)unitOfWork.GetOrAddContext(key, () => CreateDbContextWithTransaction(unitOfWork));
            return await Task.FromResult(context);
        }

        private TDbContext CreateDbContext()
        {
            return _serviceProvider.GetRequiredService<TDbContext>();
        }

        private TDbContext CreateDbContextWithTransaction(IUnitOfWork unitOfWork)
        {
            //注意这里从unitOfWork的ServiceProvider注入而不是当前构造中注入的,是unitOfWork的作用域
            var context = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            var activeTransaction = context.Database.CurrentTransaction?.GetDbTransaction();

            if (activeTransaction != null)
            {
                context.Database.UseTransaction(activeTransaction);
            }
            else
            {
                if (unitOfWork.Options.IsolationLevel.HasValue)
                {
                    context.Database.BeginTransaction(isolationLevel: unitOfWork.Options.IsolationLevel.Value);
                }

                else
                {
                    context.Database.BeginTransaction();
                }
            }

            return context;
        }
    }
}
