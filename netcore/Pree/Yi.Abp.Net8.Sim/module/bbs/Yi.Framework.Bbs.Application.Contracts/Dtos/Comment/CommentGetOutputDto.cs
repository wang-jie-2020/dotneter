using Volo.Abp.Application.Dtos;
using Yi.Framework.Bbs.Application.Contracts.Dtos.BbsUser;
using Yi.Framework.Rbac.Application.Contracts.Dtos.User;

namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Comment
{
    /// <summary>
    /// �����أ����ص������ۼ���
    /// </summary>
    public class CommentGetOutputDto : EntityDto<Guid>
    {

        public DateTime? CreateTime { get; set; }
        public string Content { get; set; }

        public Guid DiscussId { get; set; }


        /// <summary>
        /// �û�id����Ϊ�û�����
        /// </summary>

        public BbsUserGetOutputDto User { get; set; }
        /// <summary>
        /// ���ڵ������id
        /// </summary>
        public Guid RootId { get; set; }

        /// <summary>
        /// ���ظ���CommentId
        /// </summary>
        public Guid ParentId { get; set; }

    }
}
