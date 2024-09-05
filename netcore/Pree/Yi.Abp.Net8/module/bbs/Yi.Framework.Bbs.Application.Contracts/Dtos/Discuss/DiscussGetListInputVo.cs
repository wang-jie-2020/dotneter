using Volo.Abp.Application.Dtos;
using Yi.Framework.Bbs.Domain.Shared.Enums;

namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Discuss
{
    public class DiscussGetListInputVo : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// �����ߵ��û���
        /// </summary>
        public string? UserName { get; set; }
        public Guid? UserId { get; set; }

        public string? Title { get; set; }

        public Guid? PlateId { get; set; }

        //Ĭ�ϲ�ѯ���ö�
        public bool? IsTop { get; set; } 


        //��ѯ��ʽ
        public QueryDiscussTypeEnum Type { get; set; } = QueryDiscussTypeEnum.New;
    }
}
