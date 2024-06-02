namespace User.Functions.Options
{
    public sealed class JwtValidationOptions
    {
        public string PublicRsaKey { get; set; } = default!;
        public string ValidIssuer { get; set; } = default!;
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateLifetime { get; set; }
    }
}
