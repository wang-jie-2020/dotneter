using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.Account;

public class AuthGetListInput : PagedRequestInput
{
    public Guid? UserId { get; set; }

    public string? OpenId { get; set; }

    public string? AuthType { get; set; }
}