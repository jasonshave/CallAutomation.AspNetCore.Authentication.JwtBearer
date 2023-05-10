namespace CallAutomation.AspNetCore.Authentication.JwtBearer;

public class AcsJwtBearerOptions
{
    public string OpenIdConfigurationUrl { get; set; }

    public string ValidAudience { get; set; }

    public bool ValidateIssuerSigningKey { get; set; } = true;

    public bool ValidateLifetime { get; set; } = true;
}