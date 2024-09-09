using Volo.Abp.Application.Dtos;

namespace Yi.Framework.Ddd.Application;

[Serializable]
public class PagedRequestInput : PagedAndSortedResultRequestDto
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