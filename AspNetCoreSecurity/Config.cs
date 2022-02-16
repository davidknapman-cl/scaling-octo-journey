using Duende.IdentityServer.Models;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResource("profile", new[] {"name", "email"}),
            new IdentityResource("foo", new[] {"bar"})
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new Client[]
        {
            new Client
            {
                ClientId = "mvc1",
                RedirectUris = new[] { "https://localhost:5002/Callback"},
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = { "openid", "profile" },

                FrontChannelLogoutUri = "https://localhost:5002/LogoutNotify"
            },

            new Client
            {
                ClientId = "mvc2",
                RedirectUris = new[] { "https://localhost:5003/signin-oidc"},
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = { "openid", "profile" },

                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                FrontChannelLogoutUri = "https://localhost:5003/signout-oidc"
            }
        };
    }
}