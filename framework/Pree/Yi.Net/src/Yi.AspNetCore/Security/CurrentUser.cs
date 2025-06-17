using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Security;

public class CurrentUser : ICurrentUser, ITransientDependency
{
    private static readonly Claim[] EmptyClaimsArray = [];

    public virtual Guid? Id => _principalAccessor.Principal?.FindUserId();

    public virtual string? UserName => FindClaim(AbpClaimTypes.UserName)?.Value;

    public virtual string[] Roles => FindClaims(AbpClaimTypes.Role).Select(c => c.Value).Distinct().ToArray();

    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }

    public virtual Claim? FindClaim(string claimType)
    {
        return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public virtual Claim[] FindClaims(string claimType)
    {
        return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
    }
}
