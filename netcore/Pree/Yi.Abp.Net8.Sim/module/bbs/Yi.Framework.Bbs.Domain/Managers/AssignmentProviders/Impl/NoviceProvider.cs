using Yi.Framework.Bbs.Domain.Entities.Assignment;

namespace Yi.Framework.Bbs.Domain.Managers.AssignmentProviders;

/// <summary>
///     新手任务提供者
/// </summary>
public class NoviceProvider : IAssignmentProvider
{
    public Task<List<AssignmentDefineAggregateRoot>> GetCanReceiveListAsync(AssignmentContext context)
    {
        //新手任务是要有前置依赖关系的，链表类型依赖
        throw new NotImplementedException();
    }
}