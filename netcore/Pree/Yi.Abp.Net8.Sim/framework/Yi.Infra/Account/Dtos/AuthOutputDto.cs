using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Account.Dtos;

public class AuthOutputDto : EntityDto<Guid>
{
    public Guid UserId { get; set; }

    public string OpenId { get; set; }

    public string Name { get; set; }

    public string AuthType { get; set; }

    public DateTime CreationTime { get; set; }
}