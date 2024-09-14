using NetStone.Common.Helpers;

namespace NetStone.Api.Client.Test.Helpers;

internal static class NetStoneApiClientConfigurationHelper
{
    public static NetStoneApiClientConfiguration Get()
    {
        var apiRootUri = EnvironmentVariableHelper.Get<Uri>(EnvironmentVariables.ApiRootUri);
        var authAuthority = EnvironmentVariableHelper.Get<Uri>(EnvironmentVariables.AuthAuthority);
        var authClientId = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthClientId);
        var authClientSecret = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthClientSecret);
        var authScopes = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthScopes);
        var authScopesArray =
            authScopes.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new NetStoneApiClientConfiguration(apiRootUri, authAuthority, authClientId, authClientSecret,
            authScopesArray);
    }
}