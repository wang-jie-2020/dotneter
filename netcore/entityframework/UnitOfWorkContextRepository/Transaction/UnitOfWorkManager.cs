using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWorkContextRepository.Transaction
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly IUnitOfWorkAccessor _unitOfWorkAccessor;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWorkManager(
            IUnitOfWorkAccessor unitOfWorkAccessor,
            IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWorkAccessor = unitOfWorkAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IUnitOfWork? Current => GetCurrentUnitOfWork();

        public IUnitOfWork Begin(UnitOfWorkOptions unitOfWorkOptions, bool requiresNew = false)
        {
            if (Current != null && !requiresNew)
            {
                return new ChildUnitOfWork(Current);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.InitializeOptions(unitOfWorkOptions);
            return unitOfWork;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var unitOfWork = _unitOfWorkAccessor.UnitOfWork;
            while (unitOfWork != null && (unitOfWork.IsCompleted || unitOfWork.IsDisposed))
            {
                return unitOfWork.Outer;
            }

            return unitOfWork;
        }

        private IUnitOfWork CreateNewUnitOfWork()
        {
            /*
             *  UnitOfWork域和容器域的:
             *      UnitOfWork的作用域内是单独的容器域IServiceScopeFactory.CreateScope(),只用来Resolve-DbContext实例
             *      在单独的容器域内解析对象的原因:
             *      1.DbContext的注册明显不是由UnitOfWork管理,在UnitOfWork的作用域结束时,DbContext的Dispose方法会被调用,在外部域内无法再调用
             *      2.UnitOfWork可以嵌套,一个DbContext对象很难做到
             */

            var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var outer = _unitOfWorkAccessor.UnitOfWork;

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                unitOfWork.SetOuter(outer);

                _unitOfWorkAccessor.SetUnitOfWork(unitOfWork);
                unitOfWork.OnDisposed += (sender, e) =>
                {
                    _unitOfWorkAccessor.SetUnitOfWork(outer);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}