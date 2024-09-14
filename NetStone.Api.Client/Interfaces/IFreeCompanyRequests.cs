using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Interfaces;

public interface IFreeCompanyRequests
{
    Task<FreeCompanySearchPageDto> SearchAsync(FreeCompanySearchQuery query, short page = 1);
    Task<FreeCompanyDto> GetAsync(string lodestoneId, int? maxAge = null);
    Task<FreeCompanyMembersOuterDto> GetMembersAsync(string lodestoneId, int? maxAge = null);
}