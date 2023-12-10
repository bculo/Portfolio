using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Keycloak.Common.Options;
using Microsoft.Extensions.Options;

namespace Keycloak.Common.Refit.Handlers;

public class AdminAuthHeaderHandler : DelegatingHandler
{
    private readonly KeycloakAdminApiOptions _adminOptions;
    private readonly IAuth0ClientCredentialFlowService _clientCredential;
    
    public AdminAuthHeaderHandler(IAuth0ClientCredentialFlowService clientCredential,
        IOptions<KeycloakAdminApiOptions> adminOptions)
    {
        _clientCredential = clientCredential;
        _adminOptions = adminOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await FetchToken();
        
        request.Headers.Add("Authorization", $"Bearer {token.AccessToken.Trim()}");
        
        return await base.SendAsync(request, cancellationToken);
    }
    
    private async Task<TokenClientCredentialResponse> FetchToken()
    {
        var adminTokenResponse = await _clientCredential.GetToken(_adminOptions.ClientId, _adminOptions.ClientSecret)
            .ConfigureAwait(false);

        if (adminTokenResponse is null)
        {
            throw new ArgumentNullException(nameof(adminTokenResponse));
        }
        
        return adminTokenResponse;
    }
}