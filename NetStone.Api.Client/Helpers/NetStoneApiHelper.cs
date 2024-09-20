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

internal class NetStoneApiHelper(NetStoneApiClientConfiguration configuration)
{
    private static readonly SocketsHttpHandler Handler = new() { PooledConnectionLifetime = TimeSpan.FromMinutes(60) };
    private static readonly HttpClient HttpClient = new(Handler);

    private static readonly SemaphoreSlim AccessTokenLock = new(1, 1);
    private static AuthenticationResult? _accessToken;

    private static readonly JsonSerializerOptions SearchOptions = new()
        { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public async Task<T> GetAsync<T>(Uri uri, int? maxAge, CancellationToken cancellationToken)
    {
        if (maxAge is not null)
        {
            uri = new Uri($"{uri}?maxAge={maxAge}");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        request.Headers.Authorization = await GetAuthorizationHeader(false, cancellationToken);

        return await SendAndHandleResponseAsync<T>(request, cancellationToken);
    }

    public async Task<T> SearchAsync<T, TQuery>(Uri uri, TQuery query, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        request.Headers.Authorization = await GetAuthorizationHeader(false, cancellationToken);

        var queryStr = JsonSerializer.Serialize(query, SearchOptions);
        request.Content = new StringContent(queryStr, Encoding.UTF8, "application/json");

        return await SendAndHandleResponseAsync<T>(request, cancellationToken);
    }

    private async Task<T> SendAndHandleResponseAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;

        for (var i = 0; i < 2; i++)
        {
            response = await HttpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                break;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized && i == 0)
            {
                // access token was revoked. get a new one and try again.
                request.Headers.Authorization = await GetAuthorizationHeader(true, cancellationToken);
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

        var content = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        response.Dispose();

        if (content is null)
        {
            throw new ParseFailedException();
        }

        return content;
    }

    private async Task<AuthenticationHeaderValue> GetAuthorizationHeader(bool forceRefresh,
        CancellationToken cancellationToken)
    {
        await AccessTokenLock.WaitAsync(cancellationToken);
        try
        {
            if (_accessToken is null || _accessToken.ExpiresOn.UtcDateTime < DateTime.UtcNow.AddSeconds(-30) ||
                forceRefresh)
            {
                // get new access token if none exists, if it's expired, or if it expires within the next 30 seconds
                _accessToken = await GetNewAccessTokenAsync(cancellationToken);
            }
        }
        finally
        {
            AccessTokenLock.Release();
        }

        return new AuthenticationHeaderValue("Bearer", _accessToken.AccessToken);
    }

    private async Task<AuthenticationResult> GetNewAccessTokenAsync(CancellationToken cancellationToken)
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(configuration.AuthClientId)
            .WithClientSecret(configuration.AuthClientSecret)
            .WithOidcAuthority(configuration.AuthAuthority.ToString())
            .Build();

        return await app.AcquireTokenForClient(configuration.AuthScopes).ExecuteAsync(cancellationToken);
    }
}