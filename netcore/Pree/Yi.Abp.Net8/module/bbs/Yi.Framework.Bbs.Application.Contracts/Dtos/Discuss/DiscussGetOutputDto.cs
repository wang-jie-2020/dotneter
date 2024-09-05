using Volo.Abp.Application.Dtos;
using Yi.Framework.Bbs.Application.Contracts.Dtos.BbsUser;
using Yi.Framework.Bbs.Application.Contracts.Dtos.Plate;
using Yi.Framework.Bbs.Domain.Shared.Enums;
using Yi.Framework.Rbac.Application.Contracts.Dtos.User;

namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Discuss
{
    public class DiscussGetOutputDto : EntityDto<Guid>
    {
        /// <summary>
        /// �Ƿ��ֹ���۴�������
        /// </summary>
        public bool IsDisableCreateComment { get; set; }
        public string Title { get; set; }
        public string? Types { get; set; }
        public string? Introduction { get; set; }
        public int AgreeNum { get; set; }
        public int SeeNum { get; set; }
        public string Content { get; set; }
        public string? Color { get; set; }

        public Guid PlateId { get; set; }
        //�Ƿ��ö���Ĭ��false
        public bool IsTop { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string? Cover { get; set; }
        //�Ƿ�˽�У�Ĭ��false
        public bool IsPrivate { get; set; }

        //˽����Ҫ�ж�codeȨ��
        public string? PrivateCode { get; set; }
        public DateTime CreationTime { get; set; }
        public DiscussPermissionTypeEnum PermissionType { get; set; }
        public bool IsAgree { get; set; } = false;
        public List<Guid>? PermissionUserIds { get; set; }
        public BbsUserGetListOutputDto User { get; set; }

        public PlateGetOutputDto Plate { get; set; }
    }
}
