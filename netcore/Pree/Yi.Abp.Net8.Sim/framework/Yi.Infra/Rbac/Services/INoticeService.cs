using Yi.Infra.Rbac.Dtos.Notice;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Notice服务抽象
/// </summary>
public interface INoticeService : IYiCrudAppService<NoticeGetOutputDto, NoticeGetListOutputDto, Guid, NoticeGetListInput
    , NoticeCreateInput, NoticeUpdateInput>
{
}