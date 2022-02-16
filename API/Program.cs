var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters.ValidateAudience = false;

        options.TokenValidationParameters.ValidTypes =  new[] { "at+jwt" };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("scope", policy =>
    {
        policy.RequireAuthenticatedUser()
        .RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization("scope");

app.MapGet("/", () => "Root Path");

app.Run();
