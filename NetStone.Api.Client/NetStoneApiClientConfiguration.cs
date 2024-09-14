namespace NetStone.Api.Client;

/// <summary>
///     Configuration for the NetStone API client.
/// </summary>
/// <param name="ApiRootUri">The root URI of the NetStone API the client will connect to.</param>
/// <param name="AuthAuthority">OAuth Authority URL, used to retrieve OAuth metadata.</param>
/// <param name="AuthClientId">OAuth client ID.</param>
/// <param name="AuthClientSecret">OAuth client secret.</param>
/// <param name="AuthScopes">Authorization scopes to be submitted with request.</param>
public record NetStoneApiClientConfiguration(
    Uri ApiRootUri,
    Uri AuthAuthority,
    string AuthClientId,
    string AuthClientSecret,
    string[] AuthScopes);