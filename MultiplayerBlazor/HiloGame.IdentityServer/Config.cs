using Duende.IdentityServer.Models;

namespace HiloGame.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("hilogame.api","HiloGame Multiplayer API" )
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {

               new Client
               {
                   ClientId = "hilowasm",
                   ClientName = "HiloGame WASM Client",
                   AllowedGrantTypes = GrantTypes.Code,
                   RequireClientSecret = false,
                   RequirePkce = true,
                   
                   RedirectUris = { "https://localhost:7045/authentication/login-callback" },
                   PostLogoutRedirectUris = { "https://localhost:7045/authentication/logout-callback" },
                   AllowedCorsOrigins = { "https://localhost:7045" },
                   
                   AllowedScopes = { "openid", "profile", "hilogame.api" },

                AllowOfflineAccess = true
            }
            };
    }
}
