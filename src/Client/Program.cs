// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using IdentityModel.Client;
using static System.Console;

WriteLine("Hello, World!");



var httpClient = new HttpClient();
var discoveryDocumentAsync = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
if (discoveryDocumentAsync.IsError)
{
    WriteLine("there is an error {0}", discoveryDocumentAsync.Error);
}

var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
{
    Address = discoveryDocumentAsync.TokenEndpoint,
    ClientId = "myClient",
    ClientSecret = "secret",
    Scope = "api1"
});
if (tokenResponse.IsError)
{
    WriteLine("there is an error {0}", tokenResponse.Error);

}
Console.WriteLine("token response {0}", tokenResponse.AccessToken );

var client = new HttpClient();
client.SetBearerToken(tokenResponse.AccessToken);
var httpResponseMessage = await client.GetAsync("https://localhost:6001/identity");
var rootElement = JsonDocument.Parse(await httpResponseMessage.Content.ReadAsStringAsync()).RootElement;
Console.WriteLine("sdgfsdfgsd {0}",JsonSerializer.Serialize(rootElement));