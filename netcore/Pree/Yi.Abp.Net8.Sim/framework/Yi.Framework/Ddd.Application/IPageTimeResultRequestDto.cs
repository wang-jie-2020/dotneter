using Volo.Abp.Application.Dtos;

namespace Yi.Framework.Ddd.Application;

public interface IPageTimeResultRequestDto : IPagedAndSortedResultRequest
{
    DateTime? StartTime { get; set; }
    DateTime? EndTime { get; set; }
}