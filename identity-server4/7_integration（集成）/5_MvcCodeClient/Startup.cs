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

            //�ر��� JWT Claim ����ӳ�䣬�������õ�Claim������'sub'��'idp'��
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
                options.ClientId = "client_Code";
                options.ClientSecret = "secret";    //Codeģʽ������֤ClientSecret��
                options.SaveTokens = true;

                options.ResponseType = "code";  //����ȱʡ����ֻ����code

                /*
                 *  Scope��Codeģʽ��Ҳ�Ǳ�Ҫ���ӵģ��������Ĭ�ϵ�openid��profile
                 */
                options.Scope.Add("apis.access.scope");
                options.Scope.Add("apis.full.scope");

                options.Events = new OpenIdConnectEvents()
                {
                    OnRemoteFailure = context =>    //������Ȩʧ�ܵ���������粻ͬ����Ȩ
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
