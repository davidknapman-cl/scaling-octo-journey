using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mvc1.Pages;

public class LogoutNotifyModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(string sid)
    {
        if(User.Identity!.IsAuthenticated)
        {
            var currentSid = User.FindFirst("sid")!.Value;

            if(currentSid == sid)
            {
                await HttpContext.SignOutAsync();
            }
        }

        return new NoContentResult();
    }
}
