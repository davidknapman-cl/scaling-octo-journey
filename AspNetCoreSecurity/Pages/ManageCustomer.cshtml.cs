using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreSecurity.Pages;
public class ManageCustomerModel : PageModel
{
    private readonly IAuthorizationService authorizationService;

    public ManageCustomerModel(IAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        // Get customer from storage
        var customer = new Customer { Sub = id.ToString() };
        
        if(!(await authorizationService.AuthorizeAsync(
            User, customer,"ManageCustomer")).Succeeded)
        {
            return Forbid();
        }

        return Page();
    }
}
