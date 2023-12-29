using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;    //关闭了 JWT Claim类型映射，以允许常用的Claim（例如'sub'和'idp'）

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                //options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:5001";
                options.ClientId = "client_Hybrid";
                options.ClientSecret = "secret";    //Hybrid模式下是验证ClientSecret的
                options.SaveTokens = true;

                /*
                 *  ResponseType不能缺省，且必须包含code，id_token或token中的一种，正常是id_token
                 */
                options.ResponseType = "code id_token";
                //options.ResponseType = "code token";
                //options.ResponseType = "code token";
                //options.ResponseType = "code id_token token";

                /*
                 *  Scope在Code模式下也是必要增加的，否则就是默认的openid和profile
                 */
                options.Scope.Add("apis.access.scope");
                options.Scope.Add("apis.full.scope");

                //开启token时间的校验
                //options.Scope.Add("offline_access");
                //options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
                //options.TokenValidationParameters.RequireExpirationTime = true;

                options.Events = new OpenIdConnectEvents()
                {
                    OnRemoteFailure = context =>    //处理授权失败的情况，比如不同意授权
                    {
                        context.Response.Redirect("/");
                        context.HandleResponse();
                        return Task.FromResult(0);
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
