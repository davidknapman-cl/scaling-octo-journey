using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreSecurity.Pages.Account;

public class Login : PageModel
{
    [BindProperty]
    public string UserName { get; set; }
    [BindProperty]
    public string Password { get; set; }
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }
    
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (!string.IsNullOrWhiteSpace(UserName) &&
            UserName == Password)
        {
            var claims = new[]
            {
                new Claim("sub", "1234"),
                new Claim("name", UserName),
                new Claim("email", $"{UserName}@example.com")
            };

            var identity = new ClaimsIdentity(claims, "pwd", "name", "role");
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(principal, props);

            return LocalRedirect(ReturnUrl);
        }

        return Page();
    }
}