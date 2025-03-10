using NetStone.Api.Client.Test.DataGenerators;
using NetStone.Api.Client.Test.Helpers;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;

namespace NetStone.Api.Client.Test.Tests;

public class CharacterTests
{
    private static readonly NetStoneApiClientConfiguration ClientConfig = NetStoneApiClientConfigurationHelper.Get();
    private static readonly NetStoneApiClient Client = new(ClientConfig);

    [Theory]
    [ClassData(typeof(CharacterSearchDataGenerator))]
    public async Task ClientIsReceivingCharacterSearch(CharacterSearchQuery query)
    {
        var result = await Client.Character.SearchAsync(query);
        Assert.NotNull(result);
        Assert.True(result.HasResults);
    }

    [Theory]
    [ClassData(typeof(CharacterDataGenerator))]
    public async Task ClientIsReceivingCharacters(string lodestoneId)
    {
        var result = await Client.Character.GetAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ClientIsThrowingNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(async () => await Client.Character.GetAsync("99999999", 0));
    }

    [Theory]
    [ClassData(typeof(CharacterDataGenerator))]
    public async Task ClientIsReceivingCharacterClassJobs(string lodestoneId)
    {
        var result = await Client.Character.GetClassJobsAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(CharacterDataGenerator))]
    public async Task ClientIsReceivingCharacterMinions(string lodestoneId)
    {
        if (lodestoneId is "45386124") // Testerinus Maximus, Phoenix)
        {
            // test character has no minions
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await Client.Character.GetMinionsAsync(lodestoneId, 0));
            return;
        }

        var result = await Client.Character.GetMinionsAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(CharacterDataGenerator))]
    public async Task ClientIsReceivingCharacterMounts(string lodestoneId)
    {
        if (lodestoneId is "45386124" or "28835226") // Testerinus Maximus; Hena Wilbert; both Phoenix)
        {
            // test characters have no mounts
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await Client.Character.GetMountsAsync(lodestoneId, 0));
            return;
        }

        var result = await Client.Character.GetMountsAsync(lodestoneId, 0);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(CharacterDataGenerator))]
    public async Task ClientIsReceivingCharacterAchievements(string lodestoneId)
    {
        var result = await Client.Character.GetAchievementsAsync(lodestoneId, 0);
        Assert.NotNull(result);

        // achievements seem to be private by default, so most of these lists are empty. test the ones that are not.
        if (lodestoneId is "16303557" or "19060231" or "28812634" or "3386216" or "51569642")
        {
            Assert.NotEmpty(result.Achievements);
        }
    }
}