using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Identity.Client;
using NetStone.Api.Client.Exceptions;
using NetStone.Common.Exceptions;

namespace NetStone.Api.Client.Helpers;

internal static class NetStoneApiHelper
{
    private static readonly NetStoneClientConfiguration Configuration = NetStoneClientConfigurationHelper.Get();

    private static readonly SocketsHttpHandler Handler = new() { PooledConnectionLifetime = TimeSpan.FromMinutes(60) };
    private static readonly HttpClient HttpClient = new(Handler);

    private static async Task<string> GetAccessTokenAsync()
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(Configuration.AuthClientId)
            .WithClientSecret(Configuration.AuthClientSecret)
            .WithOidcAuthority(Configuration.AuthAuthority.ToString())
            .Build();

        var result = await app.AcquireTokenForClient(Configuration.AuthScopes).ExecuteAsync();
        return result.AccessToken;
    }

    public static async Task<T> GetAsync<T>(Uri uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

        var response = await HttpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(),
                HttpStatusCode.InternalServerError => new ApiInternalServerError(response.Content.ToString() ??
                    response.StatusCode.ToString()),
                _ => new Exception(response.StatusCode.ToString())
            };
        }

        var content = await response.Content.ReadFromJsonAsync<T>();

        if (content is null)
        {
            throw new ParseFailedException();
        }

        return content;
    }
}