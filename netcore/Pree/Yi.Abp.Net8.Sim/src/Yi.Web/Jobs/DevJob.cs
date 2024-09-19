using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace Yi.Web.Jobs;

public class DevJob : QuartzBackgroundWorkerBase
{
    public DevJob()
    {
        JobDetail = JobBuilder.Create<DevJob>().WithIdentity(nameof(DevJob)).Build();

        // Trigger = TriggerBuilder.Create().WithIdentity(nameof(HelloJob)).StartNow()
        //     .WithSimpleSchedule(x => x.WithIntervalInSeconds(1000 * 60).RepeatForever())
        //     .Build();

        Trigger = TriggerBuilder.Create().WithIdentity(nameof(DevJob)).WithCronSchedule("0 0 * * * ? ").Build();
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("你好，世界");
    }
}