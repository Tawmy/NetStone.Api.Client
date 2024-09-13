using NetStone.Api.Client.Test.DataGenerators;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Test.Tests;

public class FreeCompanyTests
{
    [Theory]
    [ClassData(typeof(FreeCompanySearchDataGenerator))]
    public async Task ClientIsReceivingFreeCompanySearch(FreeCompanySearchQuery query)
    {
        var result = await NetStoneClient.FreeCompany.SearchAsync(query);
        Assert.NotNull(result);
        Assert.True(result.HasResults);
    }

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanies(string lodestoneId)
    {
        var result = await NetStoneClient.FreeCompany.GetAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanyMembers(string lodestoneId)
    {
        var result = await NetStoneClient.FreeCompany.GetMembersAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }
}