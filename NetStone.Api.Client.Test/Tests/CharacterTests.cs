using NetStone.Api.Client.Test.DataGenerators;

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
}