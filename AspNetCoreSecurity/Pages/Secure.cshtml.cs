using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreSecurity.Pages;

[Authorize]
public class Secure : PageModel
{
    public void OnGet()
    {
        
    }
}