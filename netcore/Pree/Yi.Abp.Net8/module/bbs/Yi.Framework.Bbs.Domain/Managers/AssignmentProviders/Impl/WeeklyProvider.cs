using Yi.Framework.Bbs.Domain.Shared.Enums;

namespace Yi.Framework.Bbs.Domain.Managers.AssignmentProviders;

/// <summary>
///     每周任务提供者
/// </summary>
public class WeeklyProvider : TimerProvider
{
    protected override AssignmentTypeEnum AssignmentType => AssignmentTypeEnum.Weekly;
}