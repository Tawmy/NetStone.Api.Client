using NetStone.Api.Client.Helpers;
using NetStone.Api.Client.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Requests;

internal class FreeCompanyRequests(NetStoneApiClientConfiguration configuration) : IFreeCompanyRequests
{
    private readonly NetStoneApiHelper _apiHelper = new(configuration);
    private readonly Uri _freeCompanyRootUri = new(configuration.ApiRootUri, "FreeCompany/");

    public Task<FreeCompanySearchPageDto> SearchAsync(FreeCompanySearchQuery query, short page = 1)
    {
        var uri = new Uri(_freeCompanyRootUri, $"Search?page={page}");
        return _apiHelper.SearchAsync<FreeCompanySearchPageDto, FreeCompanySearchQuery>(uri, query);
    }

    public Task<FreeCompanyDto> GetAsync(string lodestoneId, int? maxAge = null)
    {
        var uri = new Uri(_freeCompanyRootUri, $"{lodestoneId}");
        return _apiHelper.GetAsync<FreeCompanyDto>(uri, maxAge);
    }

    public Task<FreeCompanyMembersOuterDto> GetMembersAsync(string lodestoneId, int? maxAge = null)
    {
        var uri = new Uri(_freeCompanyRootUri, $"{lodestoneId}");
        return _apiHelper.GetAsync<FreeCompanyMembersOuterDto>(uri, maxAge);
    }
}