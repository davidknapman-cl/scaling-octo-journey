using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

internal class ClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        principal.Identities.Single().AddClaim(
            new Claim("now", DateTime.Now.ToString()));

        return Task.FromResult(principal);
    }
}