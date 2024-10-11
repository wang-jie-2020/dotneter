using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Services.Infra.Dtos;

public class ProfileUpdateInput
{
    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Nick { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public long? Phone { get; set; }

    public string? Introduction { get; set; }

    public string? Remark { get; set; }

    public SexEnum? Sex { get; set; }
}