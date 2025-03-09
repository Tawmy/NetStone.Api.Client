using NetStone.Api.Client.Helpers;
using NetStone.Api.Client.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Requests;

internal class CharacterRequests(NetStoneApiClientConfiguration configuration) : ICharacterRequests
{
    private readonly NetStoneApiHelper _apiHelper = new(configuration);
    private readonly Uri _characterRootUri = new(configuration.ApiRootUri, "Character/");

    public Task<CharacterSearchPageDto> SearchAsync(CharacterSearchQuery query, short page = 1,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"Search?page={page}");
        return _apiHelper.SearchAsync<CharacterSearchPageDto, CharacterSearchQuery>(uri, query, cancellationToken);
    }

    public Task<CharacterDto> GetAsync(string lodestoneId, int? maxAge = null, bool useFallback = false,
        CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"{lodestoneId}");
        return _apiHelper.GetAsync<CharacterDto>(uri, maxAge, useFallback, cancellationToken);
    }

    public Task<CharacterClassJobOuterDto> GetClassJobsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"ClassJobs/{lodestoneId}");
        return _apiHelper.GetAsync<CharacterClassJobOuterDto>(uri, maxAge, useFallback, cancellationToken);
    }

    public Task<CollectionDto<CharacterMinionDto>> GetMinionsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"Minions/{lodestoneId}");
        return _apiHelper.GetAsync<CollectionDto<CharacterMinionDto>>(uri, maxAge, useFallback, cancellationToken);
    }

    public Task<CollectionDto<CharacterMountDto>> GetMountsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"Mounts/{lodestoneId}");
        return _apiHelper.GetAsync<CollectionDto<CharacterMountDto>>(uri, maxAge, useFallback, cancellationToken);
    }

    public Task<CharacterAchievementOuterDto> GetAchievementsAsync(string lodestoneId, int? maxAge = null,
        bool useFallback = false, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(_characterRootUri, $"Achievements/{lodestoneId}");
        return _apiHelper.GetAsync<CharacterAchievementOuterDto>(uri, maxAge, useFallback, cancellationToken);
    }
}