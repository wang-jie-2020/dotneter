﻿using IdentityServer4;
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
                            ClientId = "js",
                            ClientName = "js Client",
                            //ClientSecrets = { new Secret("secret".Sha256()) },
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
                                "apis"
                            },

                            //是否允许通过浏览器得到accesstoken
                            //AllowAccessTokensViaBrowser = true,

                            //是否显示授权页，默认false
                            //RequireConsent = true,

                            //指定此客户端是否可以请求刷新令牌（正在请求offline_access作用域）
                            //AllowOfflineAccess = true,

                            //指定使用基于授权码的授权类型的客户端是否必须发送证明密钥（默认为true）
                            //RequirePkce = false,
                        }
            };
        }
    }
}
