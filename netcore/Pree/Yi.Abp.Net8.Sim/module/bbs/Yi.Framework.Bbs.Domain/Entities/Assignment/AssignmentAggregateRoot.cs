﻿using SqlSugar;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Yi.Framework.Bbs.Domain.Shared.Enums;
using Yi.Framework.Core.Data;

namespace Yi.Framework.Bbs.Domain.Entities.Assignment;

/// <summary>
/// 任务实例表
/// </summary>
[SugarTable("Assignment")]
public class AssignmentAggregateRoot : AggregateRoot<Guid>, IHasCreationTime, IOrderNum, IHasModificationTime
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public override Guid Id { get; protected set; }

    /// <summary>
    /// 任务定义ID
    /// </summary>
    public Guid AssignmentDefineId { get; set; }

    /// <summary>
    /// 任务接收者用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 当前步骤数
    /// </summary>
    public int CurrentStepNumber { get; set; }

    /// <summary>
    /// 总共步骤数
    /// </summary>
    public int TotalStepNumber { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public AssignmentStateEnum AssignmentState { get; set; }

    /// <summary>
    /// 任务奖励的钱钱数量
    /// </summary>
    public decimal RewardsMoneyNumber { get; set; }
    /// <summary>
    /// 任务过期时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    public DateTime? CompleteTime { get; set; }
    
    
    public DateTime CreationTime { get; }
    public int OrderNum { get; set; }
    public DateTime? LastModificationTime { get; }


    public bool IsAllowCompleted()
    {
        return AssignmentState == AssignmentStateEnum.Progress && this.CurrentStepNumber == this.TotalStepNumber;
    }

    public bool TrySetExpire()
    {
        if (ExpireTime<=DateTime.Now)
        {
            AssignmentState = AssignmentStateEnum.Expired;
            return false;
        }

        return true;
    }
}