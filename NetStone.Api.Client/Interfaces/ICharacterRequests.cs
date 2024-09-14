using NetStone.Common.DTOs.Character;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Interfaces;

public interface ICharacterRequests
{
    Task<CharacterSearchPageDto> SearchAsync(CharacterSearchQuery query, short page = 1);
    Task<CharacterDto> GetAsync(string lodestoneId, int? maxAge = null);
    Task<CharacterClassJobOuterDto> GetClassJobsAsync(string lodestoneId, int? maxAge = null);
    Task<CharacterMinionOuterDto> GetMinionsAsync(string lodestoneId, int? maxAge = null);
    Task<CharacterMountOuterDto> GetMountsAsync(string lodestoneId, int? maxAge = null);
    Task<CharacterAchievementOuterDto> GetAchievementsAsync(string lodestoneId, int? maxAge = null);
}