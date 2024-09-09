using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Infra.Rbac.Dtos.Account;

public class AuthGetListInput : PagedAllResultRequestDto
{
    public Guid? UserId { get; set; }

    public string? OpenId { get; set; }

    public string? AuthType { get; set; }
}