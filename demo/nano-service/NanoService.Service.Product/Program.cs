using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using NanoService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddApollo(builder.Configuration.GetSection("apollo"))
//    .AddDefault()
//    .AddNamespace("nano-product");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
   {
       option.Authority = builder.Configuration["auth:authority"];
       option.Audience = builder.Configuration["auth:audience"];
       option.RequireHttpsMetadata = false;
   });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "",
        Version = "v1",
        Description = ""
    });
    options.SwaggerDoc("v1-gateway", new OpenApiInfo()
    {
        Title = "",
        Version = "v1-gateway",
        Description = ""
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Password = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{builder.Configuration["auth:authority"]}/connect/authorize"),
                Scopes = new Dictionary<string, string>
                {
                    {"customer.scope", "customer的scope"},
                    {"product.scope", "product的scope"}

                },
                TokenUrl = new Uri($"{builder.Configuration["auth:authority"]}/connect/token"),
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();


app.UseSwagger(c =>
{
    //网关中显示的swagger的调用尝试方式二，增加一个前缀，体验感一言难尽
    c.PreSerializeFilters.Add((doc, req) =>
    {
        if (doc.Info.Version.EndsWith("gateway"))
        {
            OpenApiPaths paths = new OpenApiPaths();

            foreach (var p in doc.Paths)
            {
                paths.Add("/product" + p.Key, p.Value);
            }

            doc.Paths = paths;
        }
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
    c.SwaggerEndpoint("/swagger/v1-gateway/swagger.json", "Product API v1-gateway");

    c.OAuthClientId("client_resourceOwnerPassword");
    c.OAuthClientSecret("secret");
    c.OAuthUsePkce();
});

app.UseHealthChecks("/healthz");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.Map("/apollo", b =>
//{
//    b.Run(async handler =>
//    {
//        var result = app.Configuration["product.health"];
//        await handler.Response.WriteAsync(result);
//    });
//});


app.UseConsul(app.Configuration);

app.Run();