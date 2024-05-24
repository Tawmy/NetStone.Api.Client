using NetStone.Api.Client.Test.DataGenerators;

namespace NetStone.Api.Client.Test.Tests;

public class FreeCompanyTests
{
    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanies(string lodestoneId)
    {
        var result = await NetStoneClient.FreeCompany.GetAsync(lodestoneId);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ClientIsReceivingFreeCompanyMembers(string lodestoneId)
    {
        var result = await NetStoneClient.FreeCompany.GetMembersAsync(lodestoneId);
        Assert.NotNull(result);
    }
}