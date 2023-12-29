using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkContextRepository.Transaction
{


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    /*
        IUnitOfWork:
        1.作用域,是一个IDisposable对象
        2.配置,注入默认配置和标注/传入配置
        3.在作用域范围内的DbContext集合(和ContextProver的交互通过manager实现)
        4.提交操作、回滚操作以及状态属性
        5.结束事件、失败事件、Dispose事件
	/*	
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    public interface IUnitOfWork : IDisposable
    {
        IServiceProvider ServiceProvider { get; }

        IUnitOfWork Outer { get; }

        void SetOuter(IUnitOfWork outer);

        Guid Id { get; }

        UnitOfWorkOptions Options { get; }

        void InitializeOptions(UnitOfWorkOptions options);

        void AddContext(string key, DbContext context);

        DbContext GetOrAddContext(string key, Func<DbContext> factory);

        Task CompleteAsync();

        Task RollbackAsync();

        bool IsCompleted { get; }

        void OnCompleted(Func<Task> handler);

        bool IsDisposed { get; }

        event EventHandler<UnitOfWorkFailedEventArgs> OnFailed;

        event EventHandler<UnitOfWorkEventArgs> OnDisposed;
    }
}
