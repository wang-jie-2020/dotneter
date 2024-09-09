using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Yi.Framework.Authentication.OAuth.Gitee.GiteeAuthenticationConstants;

namespace Yi.Framework.Authentication.OAuth.Gitee;

public class GiteeAuthenticationHandler : OauthAuthenticationHandler<GiteeAuthenticationOptions>
{
    public GiteeAuthenticationHandler(IOptionsMonitor<GiteeAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, IHttpClientFactory httpClientFactory) : base(options, logger, encoder, httpClientFactory)
    {
    }

    public override string AuthenticationSchemeNmae => GiteeAuthenticationDefaults.AuthenticationScheme;

    protected override async Task<List<Claim>> GetAuthTicketAsync(string code)
    {
        //获取 accessToken
        var tokenQueryKv = new List<KeyValuePair<string, string?>>
        {
            new("grant_type", "authorization_code"),
            new("client_id", Options.ClientId),
            new("client_secret", Options.ClientSecret),
            new("redirect_uri", Options.RedirectUri),
            new("code", code)
        };
        var tokenModel =
            await SendHttpRequestAsync<GiteeAuthticationcationTokenResponse>(GiteeAuthenticationDefaults.TokenEndpoint,
                tokenQueryKv, HttpMethod.Post);

        //获取 userInfo
        var userInfoQueryKv = new List<KeyValuePair<string, string?>>
        {
            new("access_token", tokenModel.access_token)
        };
        var userInfoMdoel =
            await SendHttpRequestAsync<GiteeAuthticationcationUserInfoResponse>(
                GiteeAuthenticationDefaults.UserInformationEndpoint, userInfoQueryKv);

        var claims = new List<Claim>
        {
            new(Claims.AvatarUrl, userInfoMdoel.avatar_url),
            new(Claims.Url, userInfoMdoel.url),

            new(AuthenticationConstants.OpenId, userInfoMdoel.id.ToString()),
            new(AuthenticationConstants.Name, userInfoMdoel.name),
            new(AuthenticationConstants.AccessToken, tokenModel.access_token)
        };
        return claims;
    }
}