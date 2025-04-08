using Microsoft.OpenApi.Models;

namespace Hugin
{
    public static class ApiGroups
    {
        const string ApiVersion = "1.0.0";

        public const string Abp = "abp";

        public const string Platform = "platform";

        public const string Terminal = "terminal";

        public const string Identity = "identity";

        public static readonly ApiInfo AbpGroup = new ApiInfo
        {
            Name = ApiGroups.Abp,
            DisplayName = "ABP API",
            OpenApiInfo = new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiGroups.Abp + " api",
                Description = "ABP"
            }
        };

        public static readonly ApiInfo IdentityGroup = new ApiInfo
        {
            Name = ApiGroups.Identity,
            DisplayName = "Identity API",
            OpenApiInfo = new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiGroups.Identity + " api",
                Description = "Identity"
            }
        };

        public static readonly ApiInfo PlatformGroup = new ApiInfo
        {
            Name = ApiGroups.Platform,
            DisplayName = "Platform API",
            OpenApiInfo = new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiGroups.Platform + " api",
                Description = "Platform"
            }
        };

        public static readonly ApiInfo TerminalGroup = new ApiInfo
        {
            Name = ApiGroups.Terminal,
            DisplayName = "Terminal API",
            OpenApiInfo = new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiGroups.Terminal + " api",
                Description = "Terminal"
            }
        };
    }
}
