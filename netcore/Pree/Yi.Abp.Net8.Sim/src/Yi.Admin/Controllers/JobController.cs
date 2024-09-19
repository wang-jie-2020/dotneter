using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl.Matchers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Timing;
using Yi.Admin.Domains;
using Yi.Admin.Services.Monitor;
using Yi.Admin.Services.Monitor.Dtos;

namespace Yi.Admin.Controllers;

[ApiController]
[Route("api/app/job")]
public class JobController : AbpController
{
    private readonly IClock _clock;
    private readonly ISchedulerFactory _schedulerFactory;

    public JobController(ISchedulerFactory schedulerFactory, IClock clock)
    {
        _clock = clock;
        _schedulerFactory = schedulerFactory;
    }

    [HttpGet("{jobId}")]
    public async Task<JobGetOutput> GetAsync([FromRoute] string jobId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var jobDetail = await scheduler.GetJobDetail(new JobKey(jobId));
        var trigger = (await scheduler.GetTriggersOfJob(new JobKey(jobId))).First();
        var state = await scheduler.GetTriggerState(trigger.Key);

        var output = new JobGetOutput
        {
            JobId = jobDetail.Key.Name,
            GroupName = jobDetail.Key.Group,
            JobType = jobDetail.JobType.Name,
            Properties = JsonConvert.SerializeObject(jobDetail.JobDataMap),
            Concurrent = !jobDetail.ConcurrentExecutionDisallowed,
            Description = jobDetail.Description,
            LastRunTime = _clock.Normalize(trigger.GetPreviousFireTimeUtc()?.DateTime ?? DateTime.MinValue),
            NextRunTime = _clock.Normalize(trigger.GetNextFireTimeUtc()?.DateTime ?? DateTime.MinValue),
            AssemblyName = jobDetail.JobType.Assembly.GetName().Name,
            Status = state.ToString()
        };

        if (trigger is ISimpleTrigger simple)
        {
            output.TriggerArgs = Math.Round(simple.RepeatInterval.TotalMinutes, 2) + "分钟";
            output.Type = JobTypeEnum.Millisecond;
            output.Millisecond = simple.RepeatInterval.TotalMilliseconds;
        }
        else if (trigger is ICronTrigger cron)
        {
            output.TriggerArgs = cron.CronExpressionString!;
            output.Type = JobTypeEnum.Cron;
            output.Cron = cron.CronExpressionString;
        }

        return output;
    }

    [HttpGet]
    public async Task<PagedResultDto<JobGetListOutput>> GetListAsync([FromQuery] JobGetListInput input)
    {
        var items = new List<JobGetOutput>();

        var scheduler = await _schedulerFactory.GetScheduler();
        var groups = await scheduler.GetJobGroupNames();

        foreach (var groupName in groups)
        foreach (var jobKey in await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)))
        {
            var jobName = jobKey.Name;
            var jobGroup = jobKey.Group;
            var triggers = (await scheduler.GetTriggersOfJob(jobKey)).First();
            items.Add(await GetAsync(jobName));
        }

        var output = items.Skip((input.SkipCount - 1) * input.MaxResultCount).Take(input.MaxResultCount)
            .OrderByDescending(x => x.LastRunTime)
            .ToList();

        return new PagedResultDto<JobGetListOutput>(items.Count(), output.Adapt<List<JobGetListOutput>>());
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] JobCreateInput input)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        //设置启动时执行一次，然后最大只执行一次

        //jobBuilder
        var jobClassType = Assembly.Load(input.AssemblyName).GetTypes().FirstOrDefault(x => x.Name == input.JobType);

        if (jobClassType is null) throw new UserFriendlyException($"程序集：{input.AssemblyName}，{input.JobType} 不存在");

        var jobBuilder = JobBuilder.Create(jobClassType).WithIdentity(new JobKey(input.JobId, input.GroupName))
            .WithDescription(input.Description);

        if (!input.Concurrent) jobBuilder.DisallowConcurrentExecution();

        //triggerBuilder
        TriggerBuilder triggerBuilder = null;
        switch (input.Type)
        {
            case JobTypeEnum.Cron:
                triggerBuilder =
                    TriggerBuilder.Create()
                        .WithCronSchedule(input.Cron);


                break;
            case JobTypeEnum.Millisecond:
                triggerBuilder =
                    TriggerBuilder.Create().StartNow()
                        .WithSimpleSchedule(x => x
                            .WithInterval(TimeSpan.FromMilliseconds(input.Millisecond ?? 10000))
                            .RepeatForever()
                        );
                break;
        }

        //作业计划,单个jobBuilder与多个triggerBuilder组合
        await scheduler.ScheduleJob(jobBuilder.Build(), triggerBuilder.Build());
    }

    [HttpPut("{jobId}")]
    public async Task UpdateAsync([FromRoute] string jobId, [FromBody] JobUpdateInput input)
    {
        await DeleteAsync(new List<string> { jobId });
        await CreateAsync(input.Adapt<JobCreateInput>());
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<string> id)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.DeleteJobs(id.Select(x => new JobKey(x)).ToList());
    }

    [HttpPut("pause/{jobId}")]
    public async Task PauseAsync([FromRoute] string jobId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.PauseJob(new JobKey(jobId));
    }

    [HttpPut("/start/{jobId}")]
    public async Task StartAsync([FromRoute] string jobId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ResumeJob(new JobKey(jobId));
    }

    [HttpPost("run-once/{id}")]
    public async Task RunOnceAsync([FromRoute] string id)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var jobDetail = await scheduler.GetJobDetail(new JobKey(id));

        var jobBuilder = JobBuilder.Create(jobDetail.JobType).WithIdentity(new JobKey(Guid.NewGuid().ToString()));
        //设置启动时执行一次，然后最大只执行一次
        var trigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToString()).StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInHours(1)
                .WithRepeatCount(1))
            .Build();

        await scheduler.ScheduleJob(jobBuilder.Build(), trigger);
    }
}