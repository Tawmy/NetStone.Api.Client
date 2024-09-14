using NetStone.Api.Client.Interfaces;
using NetStone.Api.Client.Requests;

namespace NetStone.Api.Client;

public class NetStoneApiClient(NetStoneApiClientConfiguration configuration)
{
    public readonly ICharacterRequests Character = new CharacterRequests(configuration);
    public readonly IFreeCompanyRequests FreeCompany = new FreeCompanyRequests(configuration);
}