using NetStone.Api.Client.Helpers;
using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Queries;

namespace NetStone.Api.Client;

public static class NetStoneClient
{
    private static readonly NetStoneClientConfiguration Configuration = NetStoneClientConfigurationHelper.Get();

    public static class Character
    {
        private static readonly Uri CharacterRootUri = new(Configuration.ApiRootUri, "Character/");

        public static Task<CharacterSearchPageDto> SearchAsync(CharacterSearchQuery query, short page = 1)
        {
            var uri = new Uri(CharacterRootUri, $"Search?page={page}");
            return NetStoneApiHelper.SearchAsync<CharacterSearchPageDto, CharacterSearchQuery>(uri, query);
        }

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

    public static class FreeCompany
    {
        private static readonly Uri FreeCompanyRootUri = new(Configuration.ApiRootUri, "FreeCompany/");

        public static Task<FreeCompanySearchPageDto> SearchAsync(FreeCompanySearchQuery query, short page = 1)
        {
            var uri = new Uri(FreeCompanyRootUri, $"Search?page={page}");
            return NetStoneApiHelper.SearchAsync<FreeCompanySearchPageDto, FreeCompanySearchQuery>(uri, query);
        }

        public static Task<FreeCompanyDto> GetAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(FreeCompanyRootUri, $"{lodestoneId}");
            return NetStoneApiHelper.GetAsync<FreeCompanyDto>(uri);
        }

        public static Task<FreeCompanyMembersOuterDto> GetMembersAsync(string lodestoneId, int? maxAge = null)
        {
            var uri = new Uri(FreeCompanyRootUri, $"{lodestoneId}");
            return NetStoneApiHelper.GetAsync<FreeCompanyMembersOuterDto>(uri);
        }
    }
}