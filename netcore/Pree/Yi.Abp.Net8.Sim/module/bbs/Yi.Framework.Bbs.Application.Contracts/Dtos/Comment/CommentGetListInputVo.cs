namespace Yi.Framework.Bbs.Application.Contracts.Dtos.Comment
{
    public class CommentGetListInputVo
    {
        public DateTime? creationTime { get; set; }
        public string? Content { get; set; }

        //Ӧ��ѡ�����Ī�������ѯ
        public Guid? DiscussId { get; set; }
    }
}
