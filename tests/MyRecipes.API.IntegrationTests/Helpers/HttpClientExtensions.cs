using System.Net.Http.Headers;

namespace MyRecipes.API.IntegrationTests.Helpers;

public static class HttpClientExtensions
{
    public static void AddAuthorizationHeader(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}