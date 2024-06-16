using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Options;
using Microsoft.Extensions.Options;

namespace Keycloak.Common.Handlers;

public class ClientCredentialAuthHandler(
    IAuth0ClientCredentialFlowService clientCredential,
    IOptions<KeycloakAdminApiOptions> adminOptions)
    : DelegatingHandler
{
    private readonly KeycloakAdminApiOptions _adminOptions = adminOptions.Value;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var token = await FetchToken();
        
        request.Headers.Add("Authorization", $"Bearer {token.AccessToken.Trim()}");
        
        return await base.SendAsync(request, cancellationToken);
    }
    
    private async Task<TokenClientCredentialResponse> FetchToken()
    {
        var adminTokenResponse = await clientCredential.GetToken(_adminOptions.ClientId, _adminOptions.ClientSecret)
            .ConfigureAwait(false);

        if (adminTokenResponse is null)
        {
            throw new ArgumentNullException(nameof(adminTokenResponse));
        }
        
        return adminTokenResponse;
    }
}