using NetStone.Common.Queries;

namespace NetStone.Api.Client.Test.DataGenerators;

public class FreeCompanySearchDataGenerator : TheoryData<FreeCompanySearchQuery>
{
    public FreeCompanySearchDataGenerator()
    {
        Add(new FreeCompanySearchQuery("Dust Bunnies", "Phoenix"));
    }
}