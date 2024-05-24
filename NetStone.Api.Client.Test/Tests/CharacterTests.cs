using NetStone.Api.Client.Test.DataGenerators;
using NetStone.Common.Exceptions;

namespace NetStone.Api.Client.Test.Tests;

public class CharacterTests
{
    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ClientIsReceivingCharacters(string lodestoneId)
    {
        var result = await NetStoneClient.Character.GetAsync(lodestoneId);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ClientIsThrowingNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(async () => await NetStoneClient.Character.GetAsync("99999999"));
    }

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ClientIsReceivingCharacterClassJobs(string lodestoneId)
    {
        var result = await NetStoneClient.Character.GetClassJobsAsync(lodestoneId);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ClientIsReceivingCharacterMinions(string lodestoneId)
    {
        if (lodestoneId == "45386124") // Testerinus Maximus, Phoenix)
        {
            // test character has no minions
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await NetStoneClient.Character.GetMinionsAsync(lodestoneId));
            return;
        }

        var result = await NetStoneClient.Character.GetMinionsAsync(lodestoneId);
        Assert.NotNull(result);
    }

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ClientIsReceivingCharacterMounts(string lodestoneId)
    {
        if (new[] { "45386124", "28835226" }.Contains(lodestoneId)) // Testerinus Maximus; Hena Wilbert; both Phoenix)
        {
            // test characters have no mounts
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await NetStoneClient.Character.GetMountsAsync(lodestoneId));
            return;
        }

        var result = await NetStoneClient.Character.GetMountsAsync(lodestoneId);
        Assert.NotNull(result);
    }
}