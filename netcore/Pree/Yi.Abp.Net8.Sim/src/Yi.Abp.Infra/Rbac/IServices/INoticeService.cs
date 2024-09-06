using Yi.Abp.Infra.Rbac.Dtos.Notice;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Notice服务抽象
    /// </summary>
    public interface INoticeService : IYiCrudAppService<NoticeGetOutputDto, NoticeGetListOutputDto, Guid, NoticeGetListInput, NoticeCreateInput, NoticeUpdateInput>
    {

    }
}
