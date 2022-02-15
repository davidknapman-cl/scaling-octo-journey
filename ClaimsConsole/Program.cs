// See https://aka.ms/new-console-template for more information

using System.Security.Claims;

var claims = new[]
    {
    new Claim ("sub", "1234"),
    new Claim ("name", "Anders Abel"),
    new Claim ("given_name", "Anders"),
    new Claim ("family_name", "Abel"),
    new Claim ("role", "identity geek")
    };

var ci = new ClaimsIdentity(claims, "known facts", "name", "role");

Console.WriteLine($"Name: {ci.Name}");
Console.WriteLine($"Name Claim Type: {ci.NameClaimType}");

var user = new ClaimsPrincipal(ci);

Console.WriteLine();
Console.WriteLine("Claims:");
foreach (var claim in user.Claims)
{
    Console.WriteLine($"{claim.Type}:{claim.Value}");
}

Console.WriteLine();
Console.WriteLine($"Are you a geek? {user.IsInRole("identity geek")}");

Console.WriteLine($"Id: {user.FindFirst("sub")!.Value}");