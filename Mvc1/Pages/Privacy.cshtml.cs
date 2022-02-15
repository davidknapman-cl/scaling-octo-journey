using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mvc1.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (User.Identity!.IsAuthenticated) return Page();

            var url = "https://localhost:5001/connect/authorize" +
                "?client_id=mvc1" +
                "&redirect_uri=https://localhost:5002/Callback" +
                "&response_type=id_token" +
                "&response_mode=form_post" +
                "&scope=openid" +
                "&nonce=random";

            return Redirect(url);
        }
    }
}