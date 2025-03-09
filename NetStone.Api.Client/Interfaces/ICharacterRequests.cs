using NetStone.Common.DTOs.Character;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Interfaces;

public interface ICharacterRequests
{
    Task<CharacterSearchPageDto> SearchAsync(CharacterSearchQuery query, short page = 1,
        CancellationToken cancellationToken = default);

    Task<CharacterDto> GetAsync(string lodestoneId, int? maxAge = null, bool useFallback = false,
        CancellationToken cancellationToken = default);

    Task<CharacterClassJobOuterDto> GetClassJobsAsync(string lodestoneId, int? maxAge = null, bool useFallback = false,
        CancellationToken cancellationToken = default);

    Task<CollectionDto<CharacterMinionDto>> GetMinionsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);

    Task<CollectionDto<CharacterMountDto>> GetMountsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);

    Task<CharacterAchievementOuterDto> GetAchievementsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default);
}