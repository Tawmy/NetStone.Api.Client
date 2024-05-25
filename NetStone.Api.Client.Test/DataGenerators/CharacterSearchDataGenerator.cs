using NetStone.Common.Queries;

namespace NetStone.Api.Client.Test.DataGenerators;

internal class CharacterSearchDataGenerator : TheoryData<CharacterSearchQuery>
{
    public CharacterSearchDataGenerator()
    {
        Add(new CharacterSearchQuery("Alyx Bergen", "Phoenix"));
    }
}