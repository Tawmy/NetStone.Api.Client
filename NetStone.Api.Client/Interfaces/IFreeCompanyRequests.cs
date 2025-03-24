using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Interfaces;

public interface IFreeCompanyRequests
{
    Task<FreeCompanySearchPageDto> SearchAsync(FreeCompanySearchQuery query, short page = 1,
        CancellationToken cancellationToken = default);

    Task<FreeCompanyDtoV3> GetAsync(string lodestoneId, int? maxAge = null,
        FallbackType useFallback = FallbackType.None, CancellationToken cancellationToken = default);

    Task<FreeCompanyMembersOuterDtoV3> GetMembersAsync(string lodestoneId, int? maxAge = null,
        FallbackType useFallback = FallbackType.None, CancellationToken cancellationToken = default);
}