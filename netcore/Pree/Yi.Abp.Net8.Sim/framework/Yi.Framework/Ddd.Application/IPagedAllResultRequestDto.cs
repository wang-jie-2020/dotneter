using Volo.Abp.Application.Dtos;

namespace Yi.Framework.Ddd.Application;

public interface IPagedAllResultRequestDto : IPageTimeResultRequestDto, IPagedAndSortedResultRequest
{
}