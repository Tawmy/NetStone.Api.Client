using NetStone.Api.Client.Test.DataGenerators;
using NetStone.Api.Client.Test.Helpers;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Test.Tests;

public class FreeCompanyTests
{
    private static readonly NetStoneApiClientConfiguration ClientConfig = NetStoneApiClientConfigurationHelper.Get();
    private static readonly NetStoneApiClient Client = new(ClientConfig);

    [Theory]
    [ClassData(typeof(FreeCompanySearchDataGenerator))]
    public async Task ClientIsReceivingFreeCompanySearch(FreeCompanySearchQuery query)
    {
        var result = await Client.FreeCompany.SearchAsync(query);
        Assert.NotNull(result);
        Assert.True(result.HasResults);
    }

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanies(string lodestoneId)
    {
        var result = await Client.FreeCompany.GetAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanyMembers(string lodestoneId)
    {
        var result = await Client.FreeCompany.GetMembersAsync(lodestoneId, 0);
        Assert.NotNull(result);
        Assert.NotEmpty(result.Members);
    }
}