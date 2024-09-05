using Volo.Abp.Application.Dtos;
using Yi.Framework.Bbs.Application.Contracts.Dtos.BbsUser;
using Yi.Framework.Bbs.Domain.Shared.Consts;
using Yi.Framework.Bbs.Domain.Shared.Enums;
using Yi.Framework.Rbac.Application.Contracts.Dtos.User;

namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Discuss
{
    public class DiscussGetListOutputDto : EntityDto<Guid>
    {
        /// <summary>
        /// �Ƿ��ֹ���۴�������
        /// </summary>
        public bool IsDisableCreateComment { get; set; }
        /// <summary>
        /// �Ƿ��ѵ��ޣ�Ĭ��δ��¼������
        /// </summary>
        public bool IsAgree { get; set; } = false;
        public string Title { get; set; }
        public string Types { get; set; }
        public string? Introduction { get; set; }

        public int AgreeNum { get; set; }
        public int SeeNum { get; set; }

        //������ѯ���������ݣ����ܿ���
        //public string Content { get; set; }
        public string? Color { get; set; }

        public Guid PlateId { get; set; }

        //�Ƿ��ö���Ĭ��false
        public bool IsTop { get; set; }

        public DiscussPermissionTypeEnum PermissionType { get; set; }
        //�Ƿ��ֹ��Ĭ��false
        public bool IsBan { get; set; }


        /// <summary>
        /// ����
        /// </summary>
        public string? Cover { get; set; }

        //˽����Ҫ�ж�codeȨ��
        public string? PrivateCode { get; set; }
        public DateTime CreationTime { get; set; }

        public List<Guid>? PermissionUserIds { get; set; }

        public BbsUserGetListOutputDto User { get; set; }

        public void SetBan()
        {
            Title = DiscussConst.Privacy;
            Introduction = "";
            Cover = null;
            //����ֹ
            IsBan = true;
        }
    }


    public static class DiscussGetListOutputDtoExtension
    {

        public static void ApplyPermissionTypeFilter(this List<DiscussGetListOutputDto> dtos, Guid userId)
        {
              dtos?.ForEach(dto =>
            {
                switch (dto.PermissionType)
                {
                    case DiscussPermissionTypeEnum.Public:
                        break;
                    case DiscussPermissionTypeEnum.Oneself:
                        //��ǰ�����ǽ��Լ��ɼ���ͬʱ���ǵ�ǰ��¼�û�
                        if (dto.User.Id != userId)
                        {
                            dto.SetBan();
                        }
                        break;
                    case DiscussPermissionTypeEnum.User:
                        //��ǰ����Ϊ���ֿɼ���ͬʱ���ǵ�ǰ��¼�û� Ҳ ���ڿɼ��û��б���
                        if (dto.User.Id != userId && !dto.PermissionUserIds.Contains(userId))
                        {
                            dto.SetBan();
                        }
                        break;
                    default:
                        break;
                }
            });
        }

    }

}
