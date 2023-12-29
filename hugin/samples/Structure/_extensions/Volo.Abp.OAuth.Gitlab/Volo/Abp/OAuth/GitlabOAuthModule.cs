using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.OAuth
{
    public class GitlabOAuthModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AuthenticationBuilder>(builder =>
            {
                builder.AddGitLab(options =>
                {
                    options.ClientId = "112df5acc6e35ca7f60f484afb6fbf5c71dd8326ca6a186f8b46fbbe7b11463a";
                    options.ClientSecret = "3f1e4f18b4b3b7465a338697806946f6aaf6c56c1b05aabff24a405982f73c3c";
                    options.TokenEndpoint = "http://gitlab.wxlgzh.cn/oauth/token";
                    options.AuthorizationEndpoint = "http://gitlab.wxlgzh.cn/oauth/authorize";
                    options.UserInformationEndpoint = "http://gitlab.wxlgzh.cn/api/v4/user";
                    options.CallbackPath = "/signin-oidc";
                });
            });
        }
    }
}
