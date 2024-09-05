using SqlSugar;
using Yi.Framework.Bbs.Domain.Entities.Assignment;
using Yi.Framework.Bbs.Domain.Shared.Enums;

namespace Yi.Framework.Bbs.Domain.Managers.AssignmentProviders;

/// <summary>
/// 循环任务提供者
/// </summary>
public abstract class TimerProvider : IAssignmentProvider
{
    /// <summary>
    /// 任务类型
    /// </summary>
    protected abstract AssignmentTypeEnum AssignmentType { get; }

    public Task<List<AssignmentDefineAggregateRoot>> GetCanReceiveListAsync(AssignmentContext context)
    {
        //先获取到对应任务定义列表
        var assignmentDefines = context.AllAssignmentDefine.Where(x => x.AssignmentType == AssignmentType).ToList();

        //满足以下1个条件
        //1：没有正在运行的
        //2: 存在已完成，但是完成时间是过期的
        var assignmentFilterIds = context.CurrentUserAssignments
            .Where(x => 
                        //正在进行的，要去掉
                        x.AssignmentState == AssignmentStateEnum.Progress||
                        //已完成，但是还没过期，要去掉
                        (x.AssignmentState == AssignmentStateEnum.Completed&&!AssignmentType.IsExpire(x.CompleteTime!.Value)))
            .Select(x => x.AssignmentDefineId)
            .ToList();

        
        
        //出去不可接收的任务，就是可接收任务
        var output = assignmentDefines.Where(x => !assignmentFilterIds.Contains(x.Id)).ToList();
        return Task.FromResult(output);
    }
    
}