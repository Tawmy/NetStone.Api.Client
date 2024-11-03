using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Identity.Client;
using NetStone.Api.Client.Exceptions;
using NetStone.Api.Client.Extensions;
using NetStone.Common.Exceptions;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace NetStone.Api.Client.Helpers;

internal class NetStoneApiHelper
{
    private static readonly SocketsHttpHandler Handler = new() { PooledConnectionLifetime = TimeSpan.FromMinutes(60) };
    private static readonly HttpClient HttpClient = new(Handler);

    private static readonly SemaphoreSlim AccessTokenLock = new(1, 1);
    private static AuthenticationResult? _accessToken;

    private static readonly JsonSerializerOptions SearchOptions = new()
        { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly NetStoneApiClientConfiguration _configuration;
    private readonly ResiliencePipeline<HttpResponseMessage> _httpClientPipeline;

    public NetStoneApiHelper(NetStoneApiClientConfiguration configuration)
    {
        _configuration = configuration;
        _httpClientPipeline = CreateResiliencePipeline();
    }

    private ResiliencePipeline<HttpResponseMessage> CreateResiliencePipeline()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();

        builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>().Handle<TimeoutRejectedException>().HandleResult(
                static result => result is
                {
                    IsSuccessStatusCode: false,
                    StatusCode: not HttpStatusCode.InternalServerError and not HttpStatusCode.NotFound
                }),
            MaxRetryAttempts = 2,
            BackoffType = DelayBackoffType.Constant,
            Delay = TimeSpan.FromMilliseconds(500),
            UseJitter = true
        });

        builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>().HandleResult(static result =>
                result.StatusCode == HttpStatusCode.Unauthorized),
            MaxRetryAttempts = 1,
            BackoffType = DelayBackoffType.Constant,
            Delay = TimeSpan.FromMilliseconds(100),
            UseJitter = true,
            OnRetry = async args => await RetrieveNewAccessTokenAsync(args.Context.CancellationToken)
        });

        builder.AddTimeout(TimeSpan.FromSeconds(30));

        return builder.Build();
    }

    public async Task<T> GetAsync<T>(Uri uri, int? maxAge, CancellationToken cancellationToken)
    {
        if (maxAge is not null)
        {
            uri = new Uri($"{uri}?maxAge={maxAge}");
        }

        return await SendAndHandleResponseAsync<T>(() => new HttpRequestMessage(HttpMethod.Get, uri),
            cancellationToken);
    }

    public async Task<T> SearchAsync<T, TQuery>(Uri uri, TQuery query, CancellationToken cancellationToken)
    {
        var queryStr = JsonSerializer.Serialize(query, SearchOptions);

        return await SendAndHandleResponseAsync<T>(() =>
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(queryStr, Encoding.UTF8, "application/json");
            return request;
        }, cancellationToken);
    }

    private async Task<T> SendAndHandleResponseAsync<T>(Func<HttpRequestMessage> createRequestFunc,
        CancellationToken cancellationToken)
    {
        if (_accessToken is null)
        {
            await RetrieveNewAccessTokenAsync(cancellationToken);
        }

        var response = await _httpClientPipeline.ExecuteAsync<HttpResponseMessage>(token =>
        {
            // requestMessage must be re-created for each request
            var request = createRequestFunc();
            request.Headers.AddApiVersion("2");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken!.AccessToken);
            return new ValueTask<HttpResponseMessage>(HttpClient.SendAsync(request, token));
        }, cancellationToken);

        if (response is null)
        {
            throw new Exception(response?.StatusCode.ToString() ?? "API Response was null.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(),
                HttpStatusCode.InternalServerError => new ApiInternalServerError(
                    await response.Content.ReadAsStringAsync(cancellationToken)),
                _ => new Exception(response.StatusCode.ToString())
            };
        }

        var content = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        response.Dispose();

        if (content is null)
        {
            throw new ParseFailedException();
        }

        return content;
    }

    private async Task RetrieveNewAccessTokenAsync(CancellationToken cancellationToken)
    {
        await AccessTokenLock.WaitAsync(cancellationToken);
        try
        {
            var app = ConfidentialClientApplicationBuilder
                .Create(_configuration.AuthClientId)
                .WithClientSecret(_configuration.AuthClientSecret)
                .WithOidcAuthority(_configuration.AuthAuthority.ToString())
                .Build();

            _accessToken = await app.AcquireTokenForClient(_configuration.AuthScopes).ExecuteAsync(cancellationToken);
        }
        finally
        {
            AccessTokenLock.Release();
        }
    }
}