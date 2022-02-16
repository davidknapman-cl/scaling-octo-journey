using AspNetCoreSecurity;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication("cookie")
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
    .AddCookie("temp")
    .AddGoogle("Google", options =>
    {
        options.ClientId = "999560008429-i4fnstq91ek0sl61pqaics7toir0php5.apps.googleusercontent.com";
        options.ClientSecret = "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";

        options.CallbackPath = "/signin-google";

        // Will use Default scheme.
        options.SignInScheme = "temp";
    });

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryClients(Config.GetClients())
    .AddTestUsers(TestUsers.Users);

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