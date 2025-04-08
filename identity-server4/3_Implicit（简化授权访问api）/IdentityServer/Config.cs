using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    /// <summary>
    /// 准备一些种子数据
    /// </summary>
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //标准 openid（subject id）
                new IdentityResources.Profile(),//name等
            };
        }

        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("apis","Apis提供的资源"),
            };
        }

        public static List<TestUser> Users()
        {
            return new List<TestUser>
            {
                        new TestUser
                        {
                            SubjectId = "1",
                            Username = "username",
                            Password = "password",
                            Claims = new []
                            {
                                new Claim("name", "Bob"),
                                new Claim("website", "https://bob.com")
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
                            ClientId = "mvc",
                            ClientName = "MVC Client",
                            ClientSecrets = { new Secret("secret".Sha256()) },

                            AllowedGrantTypes = GrantTypes.Implicit,
                    
                            //登录成功回调处理地址，处理回调返回的数据，直接用封装好的就可以，处理起来比较麻烦的
                            RedirectUris = { "https://localhost:5002/signin-oidc" },

                            //注释成功回调处理地址，同上
                            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                "apis"
                            },

                            //是否允许通过浏览器得到accesstoken
                            AllowAccessTokensViaBrowser = true,

                            //是否显示授权页，默认false
                            RequireConsent = true
                        }
            };
        }
    }
}
