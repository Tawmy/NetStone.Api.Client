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
}