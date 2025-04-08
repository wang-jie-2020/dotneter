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
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;    //�ر��� JWT Claim����ӳ�䣬�������õ�Claim������'sub'��'idp'��

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
                options.Events = new OpenIdConnectEvents()
                {
                    OnRemoteFailure = context =>
                    {
                        context.Response.Redirect("/");
                        context.HandleResponse();
                        return Task.FromResult(0);
                    }
                };
            });

            /*
             *  ��Ȼ������û��ָ��scope����Ĭ�ϵ���openid��profile
             *  ������Ĭ������������id_token��ԭ��
             *  ����ģʽ��Ҳ������access_token����������ʾ���ٿ���
             */
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
