using Microsoft.OpenApi.Models;

namespace Hugin
{
    public static class ApiGroups
    {
        const string ApiVersion = "1.0.0";

        public const string Abp = "abp";

        public const string BookStore = "bookStore";

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

        public static readonly ApiInfo BookStoreGroup = new ApiInfo
        {
            Name = ApiGroups.BookStore,
            DisplayName = "BookStore API",
            OpenApiInfo = new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiGroups.BookStore + " api",
                Description = "BookStore"
            }
        };
    }
}
