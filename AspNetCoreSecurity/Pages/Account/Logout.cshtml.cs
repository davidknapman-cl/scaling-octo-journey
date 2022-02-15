using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreSecurity.Pages.Account;

public class Logout : PageModel
{
    
    public async Task<IActionResult> OnGet()
    {
        await HttpContext.SignOutAsync();

        return Redirect("/");
    }
}