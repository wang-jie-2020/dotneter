using Volo.Abp.Application.Dtos;
using Yi.Framework.Bbs.Application.Contracts.Dtos.BbsUser;
using Yi.Framework.Rbac.Application.Contracts.Dtos.User;

namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Comment
{
    /// <summary>
    /// ���۶෴
    /// </summary>
    public class CommentGetListOutputDto : EntityDto<Guid>
    {

        public DateTime? CreationTime { get; set; }




        public string Content { get; set; }


        /// <summary>
        /// ����id
        /// </summary>
        public Guid DiscussId { get; set; }

        public Guid ParentId { get; set; }

        public Guid RootId { get; set; }

        /// <summary>
        /// �û�,�������û���Ϣ
        /// </summary>
        public BbsUserGetOutputDto CreateUser { get; set; }


        public Guid? CreatorId { get; set; }

        /// <summary>
        /// �����۵��û���Ϣ
        /// </summary>
        public BbsUserGetOutputDto CommentedUser { get; set; }


        /// <summary>
        /// �������һ�����Σ����Ǵ���һ����ά���飬��Childrenֻ���ڶ���ʱ��ֻ��һ��
        /// </summary>
        public List<CommentGetListOutputDto> Children { get; set; } = new List<CommentGetListOutputDto>();
    }
}
