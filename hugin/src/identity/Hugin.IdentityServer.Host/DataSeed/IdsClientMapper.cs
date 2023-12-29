using AutoMapper;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace Hugin.IdentityServer.DataSeed
{
    public class IdsClientMapper : Profile
    {
        public IdsClientMapper()
        {
            CreateMap<IdsClient, Client>()
                .ForMember(d => d.AllowedScopes, t => t.Ignore())
                .ForMember(d => d.ClientSecrets, t => t.Ignore())
                .ForMember(d => d.AllowedGrantTypes, t => t.Ignore())
                .ForMember(d => d.AllowedCorsOrigins, t => t.Ignore())
                .ForMember(d => d.RedirectUris, t => t.Ignore())
                .ForMember(d => d.PostLogoutRedirectUris, t => t.Ignore());
        }
    }
}