using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Options;

namespace Yi.Infra.Rbac.Jobs;

public class BackupDataBaseJob : QuartzBackgroundWorkerBase
{
    private readonly ISqlSugarDbContext _dbContext;
    private readonly IOptions<RbacOptions> _options;

    public BackupDataBaseJob(ISqlSugarDbContext dbContext, IOptions<RbacOptions> options)
    {
        _options = options;
        _dbContext = dbContext;
        JobDetail = JobBuilder.Create<BackupDataBaseJob>().WithIdentity(nameof(BackupDataBaseJob)).Build();

        //每天00点与24点进行备份
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(BackupDataBaseJob)).WithCronSchedule("0 0 0,12 * * ? ")
            .Build();
        //Trigger = TriggerBuilder.Create().WithIdentity(nameof(BackupDataBaseJob)).WithSimpleSchedule(x=>x.WithIntervalInSeconds(10)).Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        if (_options.Value.EnableDataBaseBackup)
        {
            var logger = LoggerFactory.CreateLogger<BackupDataBaseJob>();
            logger.LogWarning("正在进行数据库备份");
            _dbContext.BackupDataBase();
            logger.LogWarning("数据库备份已完成");
        }

        return Task.CompletedTask;
    }
}