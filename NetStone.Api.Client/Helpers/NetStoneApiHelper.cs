using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Identity.Client;
using NetStone.Api.Client.Exceptions;
using NetStone.Common.Exceptions;

namespace NetStone.Api.Client.Helpers;

internal static class NetStoneApiHelper
{
    private static readonly NetStoneClientConfiguration Configuration = NetStoneClientConfigurationHelper.Get();

    private static readonly SocketsHttpHandler Handler = new() { PooledConnectionLifetime = TimeSpan.FromMinutes(60) };
    private static readonly HttpClient HttpClient = new(Handler);

    private static readonly SemaphoreSlim AccessTokenLock = new(1, 1);
    private static AuthenticationResult? _accessToken;

    private static readonly JsonSerializerOptions SearchOptions = new()
        { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public static async Task<T> GetAsync<T>(Uri uri, int? maxAge)
    {
        if (maxAge is not null)
        {
            uri = new Uri($"{uri}?maxAge={maxAge}");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        request.Headers.Authorization = await GetAuthorizationHeader();

        return await SendAndHandleResponseAsync<T>(request);
    }

    public static async Task<T> SearchAsync<T, TQuery>(Uri uri, TQuery query)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        request.Headers.Authorization = await GetAuthorizationHeader();

        var queryStr = JsonSerializer.Serialize(query, SearchOptions);
        request.Content = new StringContent(queryStr, Encoding.UTF8, "application/json");

        return await SendAndHandleResponseAsync<T>(request);
    }

    private static async Task<T> SendAndHandleResponseAsync<T>(HttpRequestMessage request)
    {
        HttpResponseMessage? response = null;

        for (var i = 0; i < 2; i++)
        {
            response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                break;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized && i == 0)
            {
                // access token was revoked. get a new one and try again.
                request.Headers.Authorization = await GetAuthorizationHeader(true);
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.NotFound => new NotFoundException(),
                    HttpStatusCode.InternalServerError => new ApiInternalServerError(response.Content.ToString() ??
                        response.StatusCode.ToString()),
                    _ => new Exception(response.StatusCode.ToString())
                };
            }
        }

        if (response is null)
        {
            throw new Exception(response?.StatusCode.ToString() ?? "API Response was null.");
        }

        var content = await response.Content.ReadFromJsonAsync<T>();
        response.Dispose();

        if (content is null)
        {
            throw new ParseFailedException();
        }

        return content;
    }

    private static async Task<AuthenticationHeaderValue> GetAuthorizationHeader(bool forceRefresh = false)
    {
        await AccessTokenLock.WaitAsync();
        try
        {
            if (_accessToken is null || _accessToken.ExpiresOn.UtcDateTime < DateTime.UtcNow.AddSeconds(-30) ||
                forceRefresh)
            {
                // get new access token if none exists, if it's expired, or if it expires within the next 30 seconds
                _accessToken = await GetNewAccessTokenAsync();
            }
        }
        finally
        {
            AccessTokenLock.Release();
        }

        return new AuthenticationHeaderValue("Bearer", _accessToken.AccessToken);
    }

    private static async Task<AuthenticationResult> GetNewAccessTokenAsync()
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(Configuration.AuthClientId)
            .WithClientSecret(Configuration.AuthClientSecret)
            .WithOidcAuthority(Configuration.AuthAuthority.ToString())
            .Build();

        return await app.AcquireTokenForClient(Configuration.AuthScopes).ExecuteAsync();
    }
}