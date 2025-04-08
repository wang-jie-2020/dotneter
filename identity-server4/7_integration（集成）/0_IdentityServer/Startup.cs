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
             *  以往用的jwt的签名是对称签名
             *  var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("huginCredentials"));
             *  var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
             *  builder.AddSigningCredential(signingCredentials);
             *
             *  ids4要求必须是非对称加密
             *  if (!(credential.Key is AsymmetricSecurityKey) &&
             *      (!(credential.Key is Microsoft.IdentityModel.Tokens.JsonWebKey)
             *      || !((Microsoft.IdentityModel.Tokens.JsonWebKey)credential.Key).HasPrivateKey))
             */
            if (environment.IsDevelopment())
            {
                //临时证书，不能用于正式环境
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

            //外部登录
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
         *  ApiScope 是ids中定义的权限管道,它不必考虑有多少客户端，客户端的权限都是什么
         *
         *  ApiResource 验证权限管道的，即在客户端可以指定options.Audience = "ApiResource的名称"
         *              这样会校验token中是否存在Audience指定的scope中的至少一个，若是则验证通过
         */

        /// <summary>
        /// 身份资源
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
            
                    //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //注销成功回调处理地址
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope"
                    },

                    //是否允许通过浏览器得到accesstoken，默认false
                    AllowAccessTokensViaBrowser = true,

                    //是否显示授权页，默认false
                    RequireConsent = true
                },

                new Client
                {
                    ClientId = "client_Code",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
            
                    //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //注销成功回调处理地址
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                    },

                    //是否显示授权页，默认false
                    RequireConsent = true,
                },

                new Client
                {
                    ClientId = "client_Hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //注销成功回调处理地址
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                    },

                    //是否显示授权页，默认false
                    RequireConsent = true,

                    //指定此客户端是否可以请求刷新令牌（正在请求offline_access作用域）
                    //AllowOfflineAccess = true,
                    //AccessTokenLifetime = 20,

                    //指定使用基于授权码的授权类型的客户端是否必须发送证明密钥（默认为true）
                    //否则抛出code challenge required这个错误，未能在客户端处配置解决，暂时不考虑
                    RequirePkce = false,
                },

                new Client
                {
                    ClientId = "client_Refresh_Hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //注销成功回调处理地址
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                        //IdentityServerConstants.StandardScopes.OfflineAccess 在AllowOfflineAccess = true时即包含了此scope
                    },

                    //是否显示授权页，默认false
                    RequireConsent = true,

                    /*
                     * 指定此客户端是否可以请求刷新令牌（正在请求offline_access作用域）
                     * 但很蛋疼的是没有找到一个封装方式来以accessToken刷新accessToken，只能通过identitymodel中方法来手动刷新token
                     */
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 10,

                    //指定使用基于授权码的授权类型的客户端是否必须发送证明密钥（默认为true）
                    //解决code challenge required这个错误
                    RequirePkce = false,
                },

                new Client
                {
                    ClientId = "client_JavaScript",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    //不进行密钥验证，因为暴露在js里不太好
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,

                    //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                    RedirectUris = { "https://localhost:5003/callback.html"  },

                    //注释成功回调处理地址，同上
                    PostLogoutRedirectUris = {  "https://localhost:5003/index.html" },

                    AllowedCorsOrigins = { "https://localhost:5003" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "shared.scope",
                        "apis.access.scope",
                        "apis.full.scope",
                        //IdentityServerConstants.StandardScopes.OfflineAccess 在AllowOfflineAccess = true时即包含了此scope
                    },

                    //是否显示授权页，默认false
                    RequireConsent = true,

                    /*
                     * 指定此客户端是否可以请求刷新令牌（正在请求offline_access作用域）
                     * 但很蛋疼的是没有找到一个封装方式来以accessToken刷新accessToken，只能通过identitymodel中方法来手动刷新token
                     */
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 10,
                }
            };
        }
    }
}