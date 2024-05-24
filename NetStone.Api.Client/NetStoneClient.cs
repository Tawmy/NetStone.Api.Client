using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Identity.Client;
using NetStone.Api.Client.Helper;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;

namespace NetStone.Api.Client;

public static class NetStoneClient
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

    public static class Character
    {
        private static readonly Uri CharacterRootUri = new(Configuration.ApiRootUri, "Character/");

        public static async Task<CharacterDto> GetAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(CharacterRootUri, $"{lodestoneId}");

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException(); // TODO
            }

            var content = await response.Content.ReadFromJsonAsync<CharacterDto>();

            if (content is null)
            {
                throw new NotFoundException(); // TODO
            }

            return content;
        }
    }
}