// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using System.Net.Http.Headers;
using static System.Console;


var tokenClient = new HttpClient();
var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync(
    new ClientCredentialsTokenRequest
    {
        Address = "https://localhost:5001/connect/token",
        ClientId = "Console",
        ClientSecret = "secret",
        Scope = "api1",
    });

WriteLine($"Token Response: {(int)tokenResponse.HttpStatusCode}");
WriteLine($"Token Response JSON: { tokenResponse.Raw }");
WriteLine();

var apiClient = new HttpClient();
apiClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

var apiResult = await apiClient.GetAsync("https://localhost:5004/test");
WriteLine($"API Result: {(int)apiResult.StatusCode}");

if(apiResult.IsSuccessStatusCode)
{
    var json = await apiResult.Content.ReadAsStringAsync();
    WriteLine($"API JSON:\n {json}");
    WriteLine();
}