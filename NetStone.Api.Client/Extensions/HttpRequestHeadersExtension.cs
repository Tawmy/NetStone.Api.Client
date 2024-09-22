using System.Net.Http.Headers;

namespace NetStone.Api.Client.Extensions;

public static class HttpRequestHeadersExtension
{
    public static void AddApiVersion(this HttpRequestHeaders headers, string apiVersion)
    {
        headers.Add("X-API-Version", apiVersion);
    }
}