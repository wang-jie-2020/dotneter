using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace NanoService.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
        }

        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope>
        {
            new ApiScope(name: "customer.scope", displayName: "customer.scope"),
            new ApiScope(name: "product.scope", displayName: "product.scope"),
        };
        }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>
        {
            new ApiResource(name: "admin.resource", displayName: "customer.resource")
            {
                Scopes = new string[]
                {
                    "customer.scope",
                    "product.scope"
                }
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
                Username = "admin",
                Password = "admin",
                Claims = new[]
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
                Claims = new[]
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
                Claims = new[]
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
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "customer.scope", "product.scope" }
            },

            new Client
            {
                ClientId = "client_resourceOwnerPassword",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = { "customer.scope", "product.scope" }
            }
        };
        }
    }
}
