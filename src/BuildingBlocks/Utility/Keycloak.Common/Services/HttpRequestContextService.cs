using Microsoft.AspNetCore.Http;

namespace Keycloak.Common.Services;

public interface IHttpRequestContextService
{
    public bool HasJwtToken { get; }
    public string? Jwt { get; }

    void Init(HttpContext context);
}

public class HttpRequestContextService : IHttpRequestContextService
{
    private bool _isInitialized = false;
    
    public bool HasJwtToken => Jwt != null;
    public string? Jwt { get; private set; }
    
    public void Init(HttpContext context)
    {
        if (_isInitialized) return;

        if (context.Request.Headers["Authorization"].Count > 0)
        {
            Jwt = context.Request.Headers["Authorization"][0];
        }
        
        _isInitialized = true;
    }
}