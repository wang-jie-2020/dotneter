using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Hugin.IdentityServer.DataSeed
{
    /// <summary>
    /// <see cref="Volo.Abp.IdentityServer.Clients.Client"/>
    /// </summary>
    internal class IdsClient
    {
        public virtual string ClientId { get; set; }

        public virtual string ClientName { get; set; }

        public virtual string Description { get; set; }

        public virtual string ClientUri { get; set; }

        public virtual string LogoUri { get; set; }

        public virtual bool Enabled { get; set; } = true;

        public virtual string ProtocolType { get; set; } = IdentityServerConstants.ProtocolTypes.OpenIdConnect;

        public virtual bool RequireClientSecret { get; set; } = true;

        public virtual bool RequireConsent { get; set; } = false;

        public virtual bool AllowRememberConsent { get; set; } = true;

        public virtual bool AlwaysIncludeUserClaimsInIdToken { get; set; } = true;

        public virtual bool RequirePkce { get; set; } = false;

        public virtual bool AllowPlainTextPkce { get; set; }

        public virtual bool RequireRequestObject { get; set; }

        public virtual bool AllowAccessTokensViaBrowser { get; set; }

        public virtual string FrontChannelLogoutUri { get; set; }

        public virtual bool FrontChannelLogoutSessionRequired { get; set; } = true;

        public virtual string BackChannelLogoutUri { get; set; }

        public virtual bool BackChannelLogoutSessionRequired { get; set; } = true;

        public virtual bool AllowOfflineAccess { get; set; } = true;

        public virtual int IdentityTokenLifetime { get; set; } = 300;

        public virtual string AllowedIdentityTokenSigningAlgorithms { get; set; }

        public virtual int AccessTokenLifetime { get; set; } = 3600;

        public virtual int AuthorizationCodeLifetime { get; set; } = 300;

        public virtual int? ConsentLifetime { get; set; }

        public virtual int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        public virtual int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        public virtual int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;

        public virtual bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public virtual int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;

        public virtual int AccessTokenType { get; set; } = (int)IdentityServer4.Models.AccessTokenType.Jwt;

        public virtual bool EnableLocalLogin { get; set; } = true;

        public virtual bool IncludeJwtId { get; set; }

        public virtual bool AlwaysSendClientClaims { get; set; }

        public virtual string ClientClaimsPrefix { get; set; } = "client_";

        public virtual string PairWiseSubjectSalt { get; set; }

        public virtual int? UserSsoLifetime { get; set; }

        public virtual string UserCodeType { get; set; }

        public virtual int DeviceCodeLifetime { get; set; } = 300;

        public virtual List<string> AllowedScopes { get; set; } = new List<string>();

        public virtual List<string> ClientSecrets { get; set; } = new List<string>();

        public virtual List<string> AllowedGrantTypes { get; set; } = new List<string>();

        public virtual List<string> AllowedCorsOrigins { get; set; } = new List<string>();

        public virtual List<string> RedirectUris { get; set; } = new List<string>();

        public virtual List<string> PostLogoutRedirectUris { get; set; } = new List<string>();

        //public virtual List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }

        //public virtual List<Volo.Abp.IdentityServer.Clients.ClientClaim> Claims { get; set; }

        //public virtual List<ClientProperty> Properties { get; set; }

        public IdsClient()
        {

        }
    }
}
