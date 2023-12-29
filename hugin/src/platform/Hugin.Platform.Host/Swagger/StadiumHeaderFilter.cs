using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Hugin.Platform.Swagger
{
    public class StadiumHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                if (descriptor.RouteValues.ContainsKey("area") && descriptor.RouteValues["area"] == "stadium")
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "__stadium",
                        Description = "场馆Id",
                        In = ParameterLocation.Header
                    });
                }
            }
        }
    }
}
