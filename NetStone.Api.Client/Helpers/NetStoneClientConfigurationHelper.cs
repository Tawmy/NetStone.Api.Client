using NetStone.Common.Helpers;

namespace NetStone.Api.Client.Helpers;

internal static class NetStoneClientConfigurationHelper
{
    public static NetStoneClientConfiguration Get()
    {
        var apiRootUri = EnvironmentVariableHelper.Get<Uri>(EnvironmentVariables.ApiRootUri);
        var authAuthority = EnvironmentVariableHelper.Get<Uri>(EnvironmentVariables.AuthAuthority);
        var authClientId = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthClientId);
        var authClientSecret = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthClientSecret);
        var authScopes = EnvironmentVariableHelper.Get(EnvironmentVariables.AuthScopes);
        var authScopesArray =
            authScopes.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new NetStoneClientConfiguration(apiRootUri, authAuthority, authClientId, authClientSecret,
            authScopesArray);
    }
}