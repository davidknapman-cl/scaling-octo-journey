using AspNetCoreSecurity;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
})
    .AddCookie("cookie", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.Cookie.Name = "demo-cookie";

        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
        {
            OnSigningOut = e =>
            {
                // Release reserved items.
                return Task.CompletedTask;
            },
            OnValidatePrincipal = e =>
            {
                // User still active?
                // Refresh roles/claims?

                return Task.CompletedTask;
            }
        };
    })
    .AddGoogle("Google", options =>
    {
        options.ClientId = "999560008429-i4fnstq91ek0sl61pqaics7toir0php5.apps.googleusercontent.com";
        options.ClientSecret = "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";

        options.CallbackPath = "/signin-google";

        // Will use Default scheme.
        //options.SignInScheme = "temp";
    })
    .AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;

        options.Authority = "https://demo.duendesoftware.com";
        options.ClientId = "login";
        options.ResponseType = "id_token";
        options.SaveTokens = true;
        options.CallbackPath = "/signin-idsrv";
        options.SignedOutCallbackPath = "/signout-callback-idsrv";
        options.RemoteSignOutPath = "/signout-idsrv";
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    })
    .AddOpenIdConnect("aad", "Azure AD", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;

        options.Authority = "https://login.windows.net/4ca9cb4c-5e5f-4be9-b700-c532992a3705";
        options.ClientId = "96e3c53e-01cb-4244-b658-a42164cb67a9";
        options.ResponseType = "id_token";
        options.CallbackPath = "/signin-aad";
        options.SignedOutCallbackPath = "/signout-callback-aad";
        options.RemoteSignOutPath = "/signout-aad";
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    })
    .AddSaml2(opt =>
    {
        opt.SPOptions.EntityId = new EntityId("https://localhost:5001/Saml2");

        var idp = new IdentityProvider(new EntityId("https://stubidp.sustainsys.com/Metadata"), opt.SPOptions)
        {
            LoadMetadata = true
        };

        opt.IdentityProviders.Add(idp);
    });


    builder.Services.AddIdentityServer(opt =>
    {
        //opt.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryClients(Config.GetClients())
    .AddTestUsers(TestUsers.Users)
    .AddInMemoryApiScopes(Config.ApiScopes);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageCustomer", policy =>
                {
                    policy.RequireAuthenticatedUser();
        //policy.RequireClaim("department", "sales");
        //policy.RequireClaim("status", "senior");
        policy.RequireAssertion(ctx =>
                    {
                        if (ctx.User.HasClaim("department", "sales")
                        && ctx.User.HasClaim("status", "senior"))
                            return true;

                        return ((Customer)ctx.Resource).Sub ==
                        ctx.User.FindFirst("sub").Value;
                    });
                });
            });

            builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            // app.UseAuthentication(); Called by UseIdentityServer
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            app.Run();