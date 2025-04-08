using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IHostEnvironment environment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //ok
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
            //});

            //ok
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Lax;
            //});

            //not ok
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            //    options.Secure = CookieSecurePolicy.SameAsRequest;
            //});

            //not ok
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //    options.Secure = CookieSecurePolicy.SameAsRequest;
            //});

            //ok
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //    options.Secure = CookieSecurePolicy.Always;
            //});

            var builder = services.AddIdentityServer(options =>
                        {
                            IdentityModelEventSource.ShowPII = true;
                        })
                        .AddInMemoryIdentityResources(Config.IdentityResources())
                        .AddInMemoryApiScopes(Config.ApiScopes())
                        .AddInMemoryApiResources(Config.ApiResources())
                        .AddInMemoryClients(Config.Clients())
                        .AddTestUsers(Config.Users());

            /*
             *  �����õ�jwt��ǩ���ǶԳ�ǩ��
             *  var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("huginCredentials"));
             *  var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
             *  builder.AddSigningCredential(signingCredentials);
             *
             *  ids4Ҫ������ǷǶԳƼ���
             *  if (!(credential.Key is AsymmetricSecurityKey) &&
             *      (!(credential.Key is Microsoft.IdentityModel.Tokens.JsonWebKey)
             *      || !((Microsoft.IdentityModel.Tokens.JsonWebKey)credential.Key).HasPrivateKey))
             */
            if (environment.IsDevelopment())
            {
                //��ʱ֤�飬����������ʽ����
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var pfxPath = Path.Combine(Path.GetDirectoryName(typeof(Startup).Assembly.Location), Configuration["Certificates:pfx"]);
                if (!File.Exists(pfxPath))
                {
                    throw new FileNotFoundException();
                }
                builder.AddSigningCredential(new X509Certificate2(pfxPath, Configuration["Certificates:pwd"]));
            }

            //�ⲿ��¼
            //services.AddAuthentication()
            //.AddGoogle("Google", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //    options.ClientId = "<insert here>";
            //    options.ClientSecret = "<insert here>";
            //})
            //.AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            //    options.SaveTokens = true;

            //    options.Authority = "https://demo.identityserver.io/";
            //    options.ClientId = "interactive.confidential";
            //    options.ClientSecret = "secret";
            //    options.ResponseType = "code";

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //})
            //.AddGitHub(options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //    options.ClientId = Configuration["GitHub:ClientId"];
            //    options.ClientSecret = Configuration["GitHub:ClientSecret"];
            //    options.EnterpriseDomain = Configuration["GitHub:EnterpriseDomain"];
            //    options.Scope.Add("user:email");
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    public class Config
    {
        /*
         *  ApiScope ��ids�ж����Ȩ�޹ܵ�,�����ؿ����ж��ٿͻ��ˣ��ͻ��˵�Ȩ�޶���ʲô
         *
         *  ApiResource ��֤Ȩ�޹ܵ��ģ����ڿͻ��˿���ָ��options.Audience = "ApiResource������"
         *              ������У��token���Ƿ����Audienceָ����scope�е�����һ������������֤ͨ��
         */

        /// <summary>
        /// �����Դ
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResources.Address()
            };
        }

        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(name:"shared.scope",displayName:"shared.scope"),
                new ApiScope(name:"apis.access.scope",displayName:"apis.access.scope"),
                new ApiScope(name:"apis.full.scope",displayName:"apis.full.scope"),
                new ApiScope(name:"platform",displayName:"platform"),
                new ApiScope(name:"terminal",displayName:"terminal")
            };
        }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>
            {
                 new ApiResource(name:"APIS",displayName:"api public resource")
                 {
                     Scopes =  new string[]
                     {
                         "shared.scope",
                         "apis.access.scope",
                         "apis.full.scope"
                     }
                 },
                 new ApiResource(name:"PLATFORM",displayName:"platform resource")
                 {
                     Scopes =  new string[]
                     {
                         "platform",
                         "terminal"
                     }
                 },
            };
        }

        public static List<TestUser> Users()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                    Claims = new []
                    {
                        new Claim("name", "admin"),
                        new Claim("role", "admin"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "operator",
                    Password = "operator",
                    Claims = new []
                    {
                        new Claim("name", "operator"),
                        new Claim("role", "operator"),
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "guest",
                    Password = "guest",
                    Claims = new []
                    {
                        new Claim("name", "guest"),
                        new Claim("role", "guest"),
                    }
                }
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client_Credential",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "shared.scope", "apis.access.scope", "apis.full.scope", "platform" }
                },

                new Client
                {
                    ClientId = "client_resourceOwnerPassword",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "shared.scope", "apis.access.scope", "apis.full.scope" }
                },

                new Client
                {
                    ClientId = "client_Implicit",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Implicit,
            
                    //��¼�ɹ��ص������ַ������ص����ص����ݣ�ֱ���÷�װ�õľͿ��ԣ����������Ƚ��鷳��
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //ע���ɹ��ص������ַ
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope"
                    },

                    //�Ƿ�����ͨ��������õ�accesstoken��Ĭ��false
                    AllowAccessTokensViaBrowser = true,

                    //�Ƿ���ʾ��Ȩҳ��Ĭ��false
                    RequireConsent = true
                },

                new Client
                {
                    ClientId = "client_Code",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
            
                    //��¼�ɹ��ص������ַ������ص����ص����ݣ�ֱ���÷�װ�õľͿ��ԣ����������Ƚ��鷳��
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //ע���ɹ��ص������ַ
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                    },

                    //�Ƿ���ʾ��Ȩҳ��Ĭ��false
                    RequireConsent = true,
                },

                new Client
                {
                    ClientId = "client_Hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    //��¼�ɹ��ص������ַ������ص����ص����ݣ�ֱ���÷�װ�õľͿ��ԣ����������Ƚ��鷳��
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //ע���ɹ��ص������ַ
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                    },

                    //�Ƿ���ʾ��Ȩҳ��Ĭ��false
                    RequireConsent = true,

                    //ָ���˿ͻ����Ƿ��������ˢ�����ƣ���������offline_access������
                    //AllowOfflineAccess = true,
                    //AccessTokenLifetime = 20,

                    //ָ��ʹ�û�����Ȩ�����Ȩ���͵Ŀͻ����Ƿ���뷢��֤����Կ��Ĭ��Ϊtrue��
                    //�����׳�code challenge required�������δ���ڿͻ��˴����ý������ʱ������
                    RequirePkce = false,
                },

                new Client
                {
                    ClientId = "client_Refresh_Hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    //��¼�ɹ��ص������ַ������ص����ص����ݣ�ֱ���÷�װ�õľͿ��ԣ����������Ƚ��鷳��
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //ע���ɹ��ص������ַ
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                        //IdentityServerConstants.StandardScopes.OfflineAccess ��AllowOfflineAccess = trueʱ�������˴�scope
                    },

                    //�Ƿ���ʾ��Ȩҳ��Ĭ��false
                    RequireConsent = true,

                    /*
                     * ָ���˿ͻ����Ƿ��������ˢ�����ƣ���������offline_access������
                     * ���ܵ��۵���û���ҵ�һ����װ��ʽ����accessTokenˢ��accessToken��ֻ��ͨ��identitymodel�з������ֶ�ˢ��token
                     */
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 10,

                    //ָ��ʹ�û�����Ȩ�����Ȩ���͵Ŀͻ����Ƿ���뷢��֤����Կ��Ĭ��Ϊtrue��
                    //���code challenge required�������
                    RequirePkce = false,
                },

                new Client
                {
                    ClientId = "client_JavaScript",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    //��������Կ��֤����Ϊ��¶��js�ﲻ̫��
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    //��¼�ɹ��ص������ַ������ص����ص����ݣ�ֱ���÷�װ�õľͿ��ԣ����������Ƚ��鷳��
                    RedirectUris = { "https://localhost:5003/callback.html"  },

                    //ע�ͳɹ��ص������ַ��ͬ��
                    PostLogoutRedirectUris = {  "https://localhost:5003/index.html" },

                    AllowedCorsOrigins = { "https://localhost:5003" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                        //IdentityServerConstants.StandardScopes.OfflineAccess ��AllowOfflineAccess = trueʱ�������˴�scope
                    },

                    //�Ƿ���ʾ��Ȩҳ��Ĭ��false
                    RequireConsent = true,

                    /*
                     * ָ���˿ͻ����Ƿ��������ˢ�����ƣ���������offline_access������
                     * ���ܵ��۵���û���ҵ�һ����װ��ʽ����accessTokenˢ��accessToken��ֻ��ͨ��identitymodel�з������ֶ�ˢ��token
                     */
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 10,
                }
            };
        }
    }
}