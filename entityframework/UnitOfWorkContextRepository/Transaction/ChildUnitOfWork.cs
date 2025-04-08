using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkContextRepository.Transaction
{
    public class ChildUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _parent;

        public ChildUnitOfWork(IUnitOfWork parent)
        {
            _parent = parent;

            _parent.OnFailed += (sender, e) =>
            {
                OnFailed?.Invoke(sender, e);
            };

            _parent.OnDisposed += (sender, e) =>
            {
                OnDisposed?.Invoke(sender, e);
            };
        }

        public UnitOfWorkOptions Options => _parent.Options;

        public bool IsCompleted => _parent.IsCompleted;

        public bool IsDisposed => _parent.IsDisposed;

        public event EventHandler<UnitOfWorkFailedEventArgs>? OnFailed;
        public event EventHandler<UnitOfWorkEventArgs>? OnDisposed;

        public Guid Id => _parent.Id;

        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        public IUnitOfWork Outer => _parent.Outer;

        public void SetOuter(IUnitOfWork outer)
        {
            _parent.SetOuter(outer);
        }

        public void InitializeOptions(UnitOfWorkOptions options)
        {
            _parent.InitializeOptions(options);
        }

        public void AddContext(string key, DbContext context)
        {
            _parent.AddContext(key, context);
        }

        public DbContext GetOrAddContext(string key, Func<DbContext> factory)
        {
            return _parent.GetOrAddContext(key, factory);
        }

        //注意这里!!!
        public async Task CompleteAsync()
        {
            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            await _parent.RollbackAsync();
        }

        public void OnCompleted(Func<Task> handler)
        {
            _parent.OnCompleted(handler);
        }

        public void Dispose()
        {

        }
    }
}
