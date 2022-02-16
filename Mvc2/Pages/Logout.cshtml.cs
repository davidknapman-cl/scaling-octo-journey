using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mvc2.Pages;
public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        return SignOut("cookie", "oidc");
    }
}
