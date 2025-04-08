using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hugin.Platform.Swagger
{
    public class EmptyDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //Console.WriteLine("EmptyDocumentFilter");
        }
    }

    public class EmptySchemaFilters : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            //Console.WriteLine("EmptySchemaFilters");
        }
    }

    public class EmptyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //Console.WriteLine("EmptyOperationFilter");
        }
    }

    public class EmptyParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            //Console.WriteLine("EmptyParameterFilter");
        }
    }

    public class EmptyRequestBodyFilter : IRequestBodyFilter
    {
        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            //Console.WriteLine("EmptyRequestBodyFilter");
        }
    }
}
