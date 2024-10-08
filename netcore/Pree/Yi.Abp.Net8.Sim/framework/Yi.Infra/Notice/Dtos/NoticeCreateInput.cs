using Yi.Infra.Notice.Entities;

namespace Yi.Infra.Notice.Dtos;

/// <summary>
///     Notice输入创建对象
/// </summary>
public class NoticeCreateInput
{
    public string Title { get; set; }
    
    public NoticeTypeEnum Type { get; set; }
    
    public string Content { get; set; }
    
    public int OrderNum { get; set; }
    
    public bool State { get; set; }
}