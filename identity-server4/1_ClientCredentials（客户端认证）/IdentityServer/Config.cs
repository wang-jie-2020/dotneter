using IdentityServer4.Models;
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
                            ClientId = "clientCredential",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret("secret".Sha256())
                            },
                            AllowedScopes = { "apis" }
                        }
            };
        }
    }
}
