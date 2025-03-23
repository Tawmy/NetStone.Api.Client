using NetStone.Api.Client.Helpers;
using NetStone.Api.Client.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Requests;

internal class FreeCompanyRequests(NetStoneApiClientConfiguration configuration) : IFreeCompanyRequests
{
    private readonly NetStoneApiHelper _apiHelper = new(configuration);
    private readonly Uri _freeCompanyRootUri = new(configuration.ApiRootUri, "FreeCompany/");

    public Task<FreeCompanySearchPageDto> SearchAsync(FreeCompanySearchQuery query, short page = 1,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_freeCompanyRootUri, $"Search?page={page}");
        return _apiHelper.SearchAsync<FreeCompanySearchPageDto, FreeCompanySearchQuery>(uri, query, cancellationToken);
    }

    public Task<FreeCompanyDtoV3> GetAsync(string lodestoneId, int? maxAge = null, bool useFallback = false,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_freeCompanyRootUri, $"{lodestoneId}");
        return _apiHelper.GetAsync<FreeCompanyDtoV3>(uri, maxAge, useFallback, cancellationToken);
    }

    public Task<FreeCompanyMembersOuterDtoV3> GetMembersAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_freeCompanyRootUri, $"Members/{lodestoneId}");
        return _apiHelper.GetAsync<FreeCompanyMembersOuterDtoV3>(uri, maxAge, useFallback, cancellationToken);
    }
}