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

            //�ر��� JWT Claim ����ӳ�䣬�������õ�Claim������'sub'��'idp'��
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    /*
                     *  ValidAudience ����������ָ֤����ApiResource�е�Scope�Ƿ���Ȩ
                     *  ��δ����ApiResource��Ҳ�������ﲻ��֤����Scopeȥ��֤
                     *  options.TokenValidationParameters.ValidateAudience = false;
                     *
                     *  ����Ч����Controller�е�ClaimsPrincipal������
                     */
                    //options.TokenValidationParameters.ValidAudience = "apis";   //��Сд����
                    options.TokenValidationParameters.ValidAudience = "APIS";
                    //options.TokenValidationParameters.ValidateAudience = false;

                    /*
                     * token��Чʱ��У������Ҫע�����jwt����һ����ƫ�ƣ�Ĭ��5min
                     * ��Ҫ�ǿ��ǵ������ӳٵȵ�����Ӱ��
                     * ������token����ʱ����5s����ôʵ������ʱ��5min+5s�ڼ�Ҳ�ǿ���ʹ�����token��
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
