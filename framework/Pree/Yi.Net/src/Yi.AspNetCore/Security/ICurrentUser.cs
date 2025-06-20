using System.Security.Claims;

namespace Yi.AspNetCore.Security;

public interface ICurrentUser
{
    Guid? Id { get; }

    string? UserName { get; }
    
    string[] Roles { get; }

    Claim? FindClaim(string claimType);
    
    Claim[] FindClaims(string claimType);
}
