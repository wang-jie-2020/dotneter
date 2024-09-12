namespace Yi.Infra.Rbac.Account.Dtos;

public class AuthGetListInput : PagedInfraInput
{
    public Guid? UserId { get; set; }

    public string? OpenId { get; set; }

    public string? AuthType { get; set; }
}