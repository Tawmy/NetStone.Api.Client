using NetStone.Common.DTOs.Character;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Interfaces;

public interface ICharacterRequests
{
    Task<CharacterSearchPageDto> SearchAsync(CharacterSearchQuery query, short page = 1,
        CancellationToken cancellationToken = default);

    Task<CharacterDtoV3> GetAsync(string lodestoneId, int? maxAge = null, bool useFallback = false,
        CancellationToken cancellationToken = default);

    Task<CharacterClassJobOuterDtoV3> GetClassJobsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false,
        CancellationToken cancellationToken = default);

    Task<CollectionDtoV3<CharacterMinionDto>> GetMinionsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);

    Task<CollectionDtoV3<CharacterMountDto>> GetMountsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);

    Task<CharacterAchievementOuterDtoV3> GetAchievementsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);
}