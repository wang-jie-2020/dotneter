using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace Hugin.IdentityServer.DataSeed
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;
        private readonly IObjectMapper<IdentityServerModule> _objectMapper;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration,
            ICurrentTenant currentTenant,
            IObjectMapper<IdentityServerModule> objectMapper)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
            _currentTenant = currentTenant;
            _objectMapper = objectMapper;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                //identityResources & identityResourceClaims
                await _identityResourceDataSeeder.CreateStandardResourcesAsync();

                //ApiScopes
                await CreateApiScopesAsync();

                //ApiResources
                await CreateApiResourcesAsync();

                //Clients
                await CreateClientsAsync();
            }
        }

        async Task CreateApiScopesAsync()
        {
            var config = _configuration.GetSection("IdentityServer:Scopes");
            foreach (var item in config.GetChildren())
            {
                var name = item["Name"];
                var displayName = item["DisplayName"];
                var description = item["Description"];

                await CreateScope(name, displayName, description);
            }

            async Task CreateScope(string name, string displayName, string description)
            {
                var apiScope = await _apiScopeRepository.GetByNameAsync(name);
                if (apiScope == null)
                {
                    apiScope = await _apiScopeRepository.InsertAsync(
                        new ApiScope(_guidGenerator.Create(), name, displayName, description),
                        true);
                }

                apiScope.DisplayName = displayName;
                apiScope.Description = description;
                await _apiScopeRepository.UpdateAsync(apiScope);
            }
        }

        private async Task CreateApiResourcesAsync()
        {
            var claims = new[]
            {
                AbpClaimTypes.Email,
                AbpClaimTypes.EmailVerified,
                AbpClaimTypes.UserName,
                AbpClaimTypes.PhoneNumber,
                AbpClaimTypes.PhoneNumberVerified,
                AbpClaimTypes.Role
            };

            var config = _configuration.GetSection("IdentityServer:Resources");
            foreach (var item in config.GetChildren())
            {
                var name = item["Name"];
                var displayName = item["DisplayName"];
                var description = item["Description"];
                var scopes = item.GetSection("Scopes").Get<string[]>();
                await CreateResource(name, displayName, description, scopes, claims);
            }

            async Task CreateResource(string name, string displayName, string description, string[] scopes, string[] claims)
            {
                var apiResource = await _apiResourceRepository.FindByNameAsync(name);
                if (apiResource == null)
                {
                    apiResource = await _apiResourceRepository.InsertAsync(
                        new ApiResource(_guidGenerator.Create(), name, displayName, description),
                        true);

                    apiResource.RemoveAllScopes();  //abp默认增加同名的resource scope，Get不到怎么想的
                }

                foreach (var scope in scopes)
                {
                    if (apiResource.FindScope(scope) == null)
                    {
                        apiResource.AddScope(scope);
                    }
                }

                foreach (var claim in claims)
                {
                    if (apiResource.FindClaim(claim) == null)
                    {
                        apiResource.AddUserClaim(claim);
                    }
                }

                await _apiResourceRepository.UpdateAsync(apiResource);
            }
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address"
            };

            var config = _configuration.GetSection("IdentityServer:Clients");
            var clients = config.Get<IdsClient[]>();

            foreach (var client in clients)
            {
                client.AllowedScopes.AddRange(commonScopes);
                await CreateClient(client);
            }

            async Task CreateClient(IdsClient idsClient)
            {
                var client = await _clientRepository.FindByClientIdAsync(idsClient.ClientId);
                if (client == null)
                {
                    client = await _clientRepository.InsertAsync(
                        new Client(_guidGenerator.Create(), idsClient.ClientId),
                        true
                    );
                }

                _objectMapper.Map<IdsClient, Client>(idsClient, client);

                foreach (var scope in idsClient.AllowedScopes)
                {
                    if (client.FindScope(scope) == null)
                    {
                        client.AddScope(scope);
                    }
                }

                foreach (var grantType in idsClient.AllowedGrantTypes)
                {
                    if (client.FindGrantType(grantType) == null)
                    {
                        client.AddGrantType(grantType);
                    }
                }

                foreach (var secret in idsClient.ClientSecrets)
                {
                    if (client.FindSecret(secret.Sha256()) == null)
                    {
                        client.AddSecret(secret.Sha256());
                    }

                }

                foreach (var redirectUri in idsClient.RedirectUris)
                {
                    if (client.FindRedirectUri(redirectUri) == null)
                    {
                        client.AddRedirectUri(redirectUri);
                    }
                }

                foreach (var postLogoutRedirectUri in idsClient.PostLogoutRedirectUris)
                {
                    if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                    {
                        client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                    }
                }

                foreach (var origin in idsClient.AllowedCorsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }

                await _clientRepository.UpdateAsync(client);
            }
        }
    }
}