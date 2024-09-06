using Yi.Abp.Infra.Rbac.Enums;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.Dtos.Notice
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class NoticeGetListInput : PagedAllResultRequestDto
    {
        public string? Title { get; set; }
        public NoticeTypeEnum? Type { get; set; }
    }
}
