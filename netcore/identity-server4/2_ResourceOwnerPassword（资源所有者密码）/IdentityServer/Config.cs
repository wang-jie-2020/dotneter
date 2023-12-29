using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    /// <summary>
    /// 准备一些种子数据
    /// </summary>
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("apis","Apis提供的资源"),
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
            {
                        new Client
                        {
                            ClientId = "resourceOwnerPassword",
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            ClientSecrets =
                            {
                                new Secret("secret".Sha256())
                            },
                            AllowedScopes = { "apis" }
                        }
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
                            Password = "password"
                        }
            };
        }
    }
}
