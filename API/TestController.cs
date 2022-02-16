using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

[Authorize]
[Route("/test")]
public class TestController : ControllerBase
{
    public IActionResult Get()
    {
        var claims = User.Claims.Select(x => x.Type + ":" + x.Value).ToArray();
        return new ObjectResult(new { message = "Hello API!", claims });
    }
}
