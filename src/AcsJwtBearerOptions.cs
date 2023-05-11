namespace CallAutomation.AspNetCore.Authentication.JwtBearer;

public class AcsJwtBearerOptions
{
    public string? OpenIdConfigurationUrl { get; set; } = "https://acscallautomation.communication.azure.com/calling/.well-known/acsopenidconfiguration";

    public string? ValidAudience { get; set; }

    public bool ValidateLifetime {get; set;} = true;
}