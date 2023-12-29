using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //关闭了 JWT Claim类型映射，以允许常用的Claim（例如'sub'和'idp'）
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";
                options.ClientId = "client_Implicit";
                options.SaveTokens = true;

                /*
                 *  Implicit模式下Access Token 和 ID Token 是同时发出的，而不是比如先拿到id_token再请求token终结点拿access_token
                 *  默认的ResponseType是id_token,对应的scope是openid和profile
                 *  可以请求token,但注意这样是需要客户端允许通过浏览器传输，同时token也相当于暴露了
                 *  个人理解这种模式下的场景就是有暴露token的需求的
                 */
                options.ResponseType = "id_token token";

                /*
                 *  Scope在token模式下也是必要增加的，否则就是默认的openid和profile
                 */
                options.Scope.Add("apis.access.scope");
                options.Scope.Add("apis.full.scope");

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
