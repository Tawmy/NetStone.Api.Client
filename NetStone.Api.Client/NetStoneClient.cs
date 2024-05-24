using NetStone.Api.Client.Helpers;
using NetStone.Common.DTOs.Character;

namespace NetStone.Api.Client;

public static class NetStoneClient
{
    private static readonly NetStoneClientConfiguration Configuration = NetStoneClientConfigurationHelper.Get();

    public static class Character
    {
        private static readonly Uri CharacterRootUri = new(Configuration.ApiRootUri, "Character/");

        public static Task<CharacterDto> GetAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(CharacterRootUri, $"{lodestoneId}");
            return NetStoneApiHelper.GetAsync<CharacterDto>(uri);
        }

        public static Task<CharacterClassJobOuterDto> GetClassJobsAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(CharacterRootUri, $"ClassJobs/{lodestoneId}");
            return NetStoneApiHelper.GetAsync<CharacterClassJobOuterDto>(uri);
        }

        public static Task<CharacterMinionOuterDto> GetMinionsAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(CharacterRootUri, $"Minions/{lodestoneId}");
            return NetStoneApiHelper.GetAsync<CharacterMinionOuterDto>(uri);
        }

        public static Task<CharacterMountOuterDto> GetMountsAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(CharacterRootUri, $"Mounts/{lodestoneId}");
            return NetStoneApiHelper.GetAsync<CharacterMountOuterDto>(uri);
        }
    }
}