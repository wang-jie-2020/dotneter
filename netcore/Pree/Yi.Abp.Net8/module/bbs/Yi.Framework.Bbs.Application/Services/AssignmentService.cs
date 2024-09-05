using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using Yi.Framework.Bbs.Application.Contracts.Dtos.Assignment;
using Yi.Framework.Bbs.Domain.Managers;

namespace Yi.Framework.Bbs.Application.Services;

/// <summary>
/// 任务系统
/// </summary>
[Authorize]
public class AssignmentService : ApplicationService
{
    private readonly AssignmentManager _assignmentManager;

    public AssignmentService(AssignmentManager assignmentManager)
    {
        _assignmentManager = assignmentManager;
    }

    /// <summary>
    /// 接收任务
    /// </summary>
    /// <param name="id"></param>
    [HttpPost("assignment/accept/{id}")]
    public async Task AcceptAsync(Guid id)
    {
        await _assignmentManager.AcceptAsync(CurrentUser.GetId(), id);
    }

    /// <summary>
    /// 接收任务奖励
    /// </summary>
    /// <param name="id"></param>
    [HttpPost("assignment/receive-rewards/{id}")]
    public async Task ReceiveRewardsAsync(Guid id)
    {
        await _assignmentManager.ReceiveRewardsAsync(id);
    }

    /// <summary>
    /// 查询任务
    /// </summary>
    public async Task<PagedResultDto<AssignmentGetListOutputDto>> GetListAsync(AssignmentGetListInput input)
    {
        throw new NotImplementedException();
    }
}