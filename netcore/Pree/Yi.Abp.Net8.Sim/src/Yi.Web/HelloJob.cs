using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace Yi.Web;

public class HelloJob : QuartzBackgroundWorkerBase
{
    public HelloJob()
    {
        JobDetail = JobBuilder.Create<HelloJob>().WithIdentity(nameof(HelloJob)).Build();

        // Trigger = TriggerBuilder.Create().WithIdentity(nameof(HelloJob)).StartNow()
        //     .WithSimpleSchedule(x => x.WithIntervalInSeconds(1000 * 60).RepeatForever())
        //     .Build();
        
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(HelloJob)).WithCronSchedule("0 0 * * * ? ").Build();
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("你好，世界");
    }
}