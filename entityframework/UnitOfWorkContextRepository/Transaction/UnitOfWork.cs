using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UnitOfWorkContextRepository.Transaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UnitOfWorkDefaultOptions _defaultOptions;
        private readonly Dictionary<string, DbContext> _databases;

        private Exception? _exception;

        private bool _isCompleting;
        private bool _isRolledback;

        public UnitOfWorkOptions Options { get; set; }

        public bool IsCompleted { get; private set; }

        public bool IsDisposed { get; private set; }

        protected List<Func<Task>> CompletedHandlers { get; }

        public event EventHandler<UnitOfWorkFailedEventArgs>? OnFailed;

        public event EventHandler<UnitOfWorkEventArgs>? OnDisposed;

        public Guid Id { get; } = Guid.NewGuid();

        public IServiceProvider ServiceProvider { get; }

        public IUnitOfWork Outer { get; private set; }

        public UnitOfWork(IServiceProvider serviceProvider, IOptions<UnitOfWorkDefaultOptions> options)
        {
            ServiceProvider = serviceProvider;
            _defaultOptions = options.Value;

            _databases = new Dictionary<string, DbContext>();
            CompletedHandlers = new List<Func<Task>>();
        }

        public void SetOuter(IUnitOfWork outer)
        {
            Outer = outer;
        }

        public void InitializeOptions(UnitOfWorkOptions options)
        {
            if (Options != null)
            {
                throw new NotSupportedException("multi-time initialize");
            }

            Options = _defaultOptions.Normalize(options);
        }

        public void AddContext(string key, DbContext context)
        {
            if (_databases.ContainsKey(key))
            {
                throw new InvalidOperationException();
            }

            _databases.Add(key, context);
        }

        public DbContext GetOrAddContext(string key, Func<DbContext> factory)
        {
            if (_databases.TryGetValue(key, out var obj))
            {
                return obj;
            }

            return _databases[key] = factory();
        }

        private IReadOnlyList<DbContext> GetAllDatabases()
        {
            return _databases.Values.ToImmutableList();
        }

        public async Task CompleteAsync()
        {
            if (_isRolledback || _isCompleting || IsCompleted)
            {
                return;
            }

            try
            {
                _isCompleting = true;
                await SaveChangesAsync();
                await CommitTransactionsAsync();
                IsCompleted = true;
                await OnCompletedAsync();
            }
            catch (Exception e)
            {
                //这里可以不写回滚语句,EF会自动进行回滚(ado还是要写的)
                _exception = e;
                throw;
            }
        }

        protected virtual async Task SaveChangesAsync()
        {
            foreach (var database in GetAllDatabases())
            {
                await database.SaveChangesAsync();
            }
        }

        protected virtual async Task CommitTransactionsAsync()
        {
            foreach (var database in GetAllDatabases())
            {
                await database.Database.CommitTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            await RollbackAllAsync();
        }

        protected virtual async Task RollbackAllAsync()
        {
            foreach (var database in GetAllDatabases())
            {
                try
                {
                    await database.Database.RollbackTransactionAsync();
                }
                catch { }
            }
        }

        public void OnCompleted(Func<Task> handler)
        {
            CompletedHandlers.Add(handler);
        }

        protected async Task OnCompletedAsync()
        {
            foreach (var handler in CompletedHandlers)
            {
                await handler.Invoke();
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            DisposeDatabase();

            if (!IsCompleted || _exception != null)
            {
                OnFailed?.Invoke(this, new UnitOfWorkFailedEventArgs(this, _exception, _isRolledback));
            }

            OnDisposed?.Invoke(this, new UnitOfWorkEventArgs(this));
        }

        protected virtual void DisposeDatabase()
        {
            foreach (var database in GetAllDatabases())
            {
                database.Dispose();
            }
        }
    }
}