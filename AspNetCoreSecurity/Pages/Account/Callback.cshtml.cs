using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AspNetCoreSecurity.Pages.Account;

[AllowAnonymous]
public class CallbackModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        var result = await HttpContext.AuthenticateAsync("temp");

        if (!result.Succeeded) throw new Exception("error");

        var user = result.Principal;

        var sub = user.FindFirst(ClaimTypes.NameIdentifier).Value;

        var issuer = result.Properties.Items["scheme"];

        // go to db, look up our own user
        // AspNetUsers - our view of users.
        // AspNetUserLogins - issuer + provider key.

        var claims = new[]
        {
            new Claim("sub", "123"),
            new Claim("name", "Anders"),
            new Claim("email", user.FindFirst(ClaimTypes.Email).Value)
        };

        var claimsIdentity = new ClaimsIdentity(claims, issuer, "name", "role");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(claimsPrincipal);
        await HttpContext.SignOutAsync("temp");

        return Redirect(result.Properties.Items["uru"]);
    }
}
