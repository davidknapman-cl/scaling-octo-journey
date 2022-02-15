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
    });

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();