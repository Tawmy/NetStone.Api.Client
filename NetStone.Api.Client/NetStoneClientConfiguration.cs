namespace NetStone.Api.Client;

internal record NetStoneClientConfiguration(
    Uri ApiRootUri,
    Uri AuthAuthority,
    string AuthClientId,
    string AuthClientSecret,
    string[] AuthScopes);