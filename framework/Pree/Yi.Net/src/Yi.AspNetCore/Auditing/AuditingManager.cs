﻿using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Threading;

namespace Yi.AspNetCore.Auditing;

public class AuditingManager : IAuditingManager, ITransientDependency
{
    private const string AmbientContextKey = "Volo.Abp.Auditing.IAuditLogScope";

    protected IServiceProvider ServiceProvider { get; }
    protected AuditingOptions Options { get; }
    protected ILogger<AuditingManager> Logger { get; set; }
    private readonly IAmbientScopeProvider<IAuditLogScope> _ambientScopeProvider;
    private readonly IAuditingHelper _auditingHelper;
    private readonly IAuditingStore _auditingStore;

    public AuditingManager(
        IAmbientScopeProvider<IAuditLogScope> ambientScopeProvider,
        IAuditingHelper auditingHelper,
        IAuditingStore auditingStore,
        IServiceProvider serviceProvider,
        IOptions<AuditingOptions> options)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        Logger = NullLogger<AuditingManager>.Instance;

        _ambientScopeProvider = ambientScopeProvider;
        _auditingHelper = auditingHelper;
        _auditingStore = auditingStore;
    }

    public IAuditLogScope? Current => _ambientScopeProvider.GetValue(AmbientContextKey);

    public IAuditLogSaveHandle BeginScope()
    {
        var ambientScope = _ambientScopeProvider.BeginScope(
            AmbientContextKey,
            new AuditLogScope(_auditingHelper.CreateAuditLogInfo())
        );

        Debug.Assert(Current != null, "Current != null");

        return new DisposableSaveHandle(this, ambientScope, Current!.Log, Stopwatch.StartNew());
    }

    protected virtual void ExecutePostContributors(AuditLogInfo auditLogInfo)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

            foreach (var contributor in Options.Contributors)
            {
                try
                {
                    contributor.PostContribute(context);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                }
            }
        }
    }

    protected virtual void BeforeSave(DisposableSaveHandle saveHandle)
    {
        saveHandle.StopWatch.Stop();
        saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
        ExecutePostContributors(saveHandle.AuditLog);
    }

    protected virtual async Task SaveAsync(DisposableSaveHandle saveHandle)
    {
        BeforeSave(saveHandle);

        await _auditingStore.SaveAsync(saveHandle.AuditLog);
    }

    protected class DisposableSaveHandle : IAuditLogSaveHandle
    {
        public AuditLogInfo AuditLog { get; }
        public Stopwatch StopWatch { get; }

        private readonly AuditingManager _auditingManager;
        private readonly IDisposable _scope;

        public DisposableSaveHandle(
            AuditingManager auditingManager,
            IDisposable scope,
            AuditLogInfo auditLog,
            Stopwatch stopWatch)
        {
            _auditingManager = auditingManager;
            _scope = scope;
            AuditLog = auditLog;
            StopWatch = stopWatch;
        }

        public async Task SaveAsync()
        {
            await _auditingManager.SaveAsync(this);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
