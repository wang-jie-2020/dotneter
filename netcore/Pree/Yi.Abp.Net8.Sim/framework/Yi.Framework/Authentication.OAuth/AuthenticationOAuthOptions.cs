using Microsoft.AspNetCore.Authentication.OAuth;

namespace Yi.Framework.Authentication.OAuth;

public class AuthenticationOAuthOptions : OAuthOptions
{
    public string RedirectUri { get; set; }
}