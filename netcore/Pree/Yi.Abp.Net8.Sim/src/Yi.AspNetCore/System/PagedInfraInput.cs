using Volo.Abp.Application.Dtos;

namespace Yi.AspNetCore.System;

/// <summary>
/// todo 明显不合适
/// </summary>
[Serializable]
public class PagedInfraInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    ///     查询开始时间条件
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    ///     查询结束时间条件
    /// </summary>
    public DateTime? EndTime { get; set; }
}