using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Yi.AspNetCore.Extensions.DependencyInjection;

public static class SwaggerGenExtensions
{
    public static IServiceCollection AddYiSwaggerGen<TModule>(this IServiceCollection services, Action<SwaggerGenOptions>? action = null)
    {
        services.AddSwaggerGen(
            options =>
            {
                action?.Invoke(options);

                options.CustomSchemaIds(type => type.FullName);
                var basePath = Path.GetDirectoryName(typeof(TModule).Assembly.Location);
                if (basePath is not null)
                    foreach (var item in Directory.GetFiles(basePath, "*.xml"))
                        options.IncludeXmlComments(item, true);

                options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme
                {
                    Description = "直接输入Token即可",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
                };
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [scheme] = Array.Empty<string>()
                });

                //options.OperationFilter<AddRequiredHeaderParameter>();
                options.SchemaFilter<EnumSchemaFilter>();
            }
        );

        return services;
    }

    public static IApplicationBuilder UseYiSwagger(this IApplicationBuilder app, Action<SwaggerUIOptions>? setupAction = null)
    {
        app.UseSwagger().UseSwaggerUI(c =>
        {
            setupAction?.Invoke(c);
        });

        return app;
    }
}

/// <summary>
///     Swagger文档枚举字段显示枚举属性和枚举值,以及枚举描述
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    ///     实现接口
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            model.Type = "string";
            model.Format = null;

            var stringBuilder = new StringBuilder();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name =>
                {
                    var e = (Enum)Enum.Parse(context.Type, name);
                    var descriptionOrNull = GetEnumDescription(e);
                    model.Enum.Add(new OpenApiString(name));
                    stringBuilder.Append(
                        $"【枚举：{name}{(descriptionOrNull is null ? string.Empty : $"({descriptionOrNull})")}={Convert.ToInt64(Enum.Parse(context.Type, name))}】<br />");
                });
            model.Description = stringBuilder.ToString();
        }
    }

    private static string? GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : null;
    }
}

public class AddRequiredHeaderParameter : IOperationFilter
{
    public static string HeaderKey { get; set; } = "__tenant";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = HeaderKey,
            In = ParameterLocation.Header,
            Required = false,
            AllowEmptyValue = true,
            Description = "租户id或者租户名称（可空为默认租户）"
        });
    }
}