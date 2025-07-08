namespace Yi.System.Services.Dtos;

public class DeptDto
{
    public Guid Id { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;

    public Guid? CreatorId { get; set; }

    public bool State { get; set; }

    public string DeptName { get; set; } = string.Empty;

    public string DeptCode { get; set; } = string.Empty;

    public string? Leader { get; set; }

    public Guid ParentId { get; set; }

    public string? Remark { get; set; }

    public int OrderNum { get; set; }
}