namespace Yi.Infra.Rbac.Dtos.Account;

public class AuthGetListInput : PagedInfraInput
{
    public Guid? UserId { get; set; }

    public string? OpenId { get; set; }

    public string? AuthType { get; set; }
}