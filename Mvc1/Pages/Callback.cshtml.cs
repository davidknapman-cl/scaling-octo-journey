using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Mvc1.Pages;

[IgnoreAntiforgeryToken]
public class CallbackModel : PageModel
{
    public async Task<IActionResult> OnPost(string id_token)
    {
        var rsaKey = new RsaSecurityKey(new RSAParameters
        {
            Exponent = Base64UrlEncoder.DecodeBytes("AQAB"),
            Modulus = Base64UrlEncoder.DecodeBytes("mG40m4r4g8TxZBQxOpgBLCXJkOuRgEiIs4Z60hxE0j9eFvIG4_9paI79O-i6ThYHkypAm_0lyUW_d3WLZ5vTL1aI-u_Ien96RMKunMrLR9ijwiv2R1_kZmNUnxBQE-kBXdhPOVKgE0IEK938WnwX9xyI1bxxCKof7Cook3QO0Zj19NBLN4AGBSjQ2gGGTVTnC9VVUO0R86vUh6KyUfNpFHjcyNEi68tyxQwfkTG4qrwvyW4uy1PgimmVrd63wtESSAI6rRA2XW3cAJiZ7fH7ibrJkYfRH6z6PEcG_8uzw6eKgMd0AUL2zVIvVgNCmvLVVSA2Wzf-EdpJJqubr6sQfQ"),
        });

        rsaKey.KeyId = "494491B0AE2A624BBC245D9F59F5074D";

        var parameters = new TokenValidationParameters
        {
            ValidAudience = "mvc1",
            ValidIssuer = "https://localhost:5001",
            IssuerSigningKey = rsaKey,
        };

        var handler = new JsonWebTokenHandler();

        var result = handler.ValidateToken(id_token, parameters);

        if (!result.IsValid) throw new Exception("error");

        if (result.ClaimsIdentity.FindFirst("nonce")!.Value != "random") throw new Exception("nonce error");

        await HttpContext.SignInAsync(
            new ClaimsPrincipal(result.ClaimsIdentity));

        return RedirectToPage("Privacy");
    }
}
