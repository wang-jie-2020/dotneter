using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Apis
{
    public class Startup
    {
        private readonly IHostEnvironment environment;

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var basePath = Path.GetDirectoryName(typeof(Startup).Assembly.Location);

            services.AddControllers();

            //关闭了 JWT Claim 类型映射，以允许常用的Claim（例如'sub'和'idp'）
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    /*
                     *  ValidAudience 的作用是验证指定的ApiResource中的Scope是否被授权
                     *  若未定义ApiResource，也可以这里不验证，以Scope去验证
                     *  options.TokenValidationParameters.ValidateAudience = false;
                     *
                     *  它的效果是Controller中的ClaimsPrincipal不正常
                     */
                    //options.TokenValidationParameters.ValidAudience = "apis";   //大小写敏感
                    options.TokenValidationParameters.ValidAudience = "APIS";
                    //options.TokenValidationParameters.ValidateAudience = false;

                    /*
                     * token有效时间校验中需要注意的是jwt容许一定的偏移，默认5min
                     * 主要是考虑到网络延迟等等因素影响
                     * 即设置token过期时间是5s，那么实际运行时，5min+5s期间也是可以使用这个token的
                     */
                    options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(0);
                    options.TokenValidationParameters.RequireExpirationTime = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SharedScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "shared.scope");
                });

                options.AddPolicy("AccessScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        if (context.User.HasClaim("scope", "apis.access.scope"))
                            return true;

                        return false;
                    });
                });

                options.AddPolicy("apis.full.scope", policy =>
                    policy.AddRequirements(new ScopeRequirement("apis.full.scope")));
            });

            services.Replace(ServiceDescriptor.Singleton<IAuthorizationPolicyProvider, LocalPolicyProvider>());
            services.AddSingleton<IAuthorizationHandler, ScopeHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var a = scope.ServiceProvider.GetServices<IAuthorizationPolicyProvider>();
                var b = scope.ServiceProvider.GetServices<IAuthorizationHandler>();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers().RequireAuthorization("ApiScope");
                endpoints.MapControllers();
            });
        }
    }
}
