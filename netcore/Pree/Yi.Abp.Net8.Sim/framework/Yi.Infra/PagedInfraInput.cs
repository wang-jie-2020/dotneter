using Volo.Abp.Application.Dtos;

namespace Yi.Infra;

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