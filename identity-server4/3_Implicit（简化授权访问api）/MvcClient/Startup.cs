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
        ///// <summary>
        ///// 上一个demo的示例
        ///// </summary>
        ///// <param name="services"></param>
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc();

        //    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;    //关闭了 JWT Claim类型映射，以允许常用的Claim（例如'sub'和'idp'）

        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultScheme = "Cookies";
        //        options.DefaultChallengeScheme = "oidc";
        //    })
        //    .AddCookie("Cookies")
        //    .AddOpenIdConnect("oidc", options =>
        //    {
        //        options.Authority = "https://localhost:5001";
        //        options.ClientId = "mvc";
        //        options.SaveTokens = true;
        //        options.Events = new OpenIdConnectEvents()
        //        {
        //            OnRemoteFailure = context =>
        //            {
        //                context.Response.Redirect("/");
        //                context.HandleResponse();
        //                return Task.FromResult(0);
        //            }
        //        };
        //    });
        //}

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
                options.Authority = "https://localhost:5001";
                options.ClientId = "mvc";
                options.SaveTokens = true;

                //通过浏览器得到token需要client允许通过浏览器传输
                options.ResponseType = "id_token token";
                //若ResponseType不包含token，那么增加scope是会出错的
                options.Scope.Add("apis");

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

        /*
         *  这种模式下Access Token 和 ID Token 都会直接发送出来，而不是比如先拿到id_token再请求token终结点拿access_token
         *  默认情况下是只请求id_token的
         *  注意：这种模式下AccessToken是通过浏览器传输的
         */

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
