using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Yi.AspNetCore
{
    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiDescriptionGroupCollectionProvider _provider;

        public SwaggerConfigureOptions(IApiDescriptionGroupCollectionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiDescriptionGroups.Items)
            {
                if (description == null) continue;
                if (description.GroupName == null) continue;

                options.SwaggerDoc(description.GroupName, null);
            }

            options.SwaggerDoc("default", null);

            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == "default")
                {
                    return true;
                }

                if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return docName == apiDesc.GroupName;
                }

                return false;
            });
        }

        public void Configure(SwaggerUIOptions options)
        {
            foreach (var description in _provider.ApiDescriptionGroups.Items)
            {
                if (description == null) continue;
                if (description.GroupName == null) continue;

                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }

            options.SwaggerEndpoint("/swagger/default/swagger.json", "default");
        }
    }
}