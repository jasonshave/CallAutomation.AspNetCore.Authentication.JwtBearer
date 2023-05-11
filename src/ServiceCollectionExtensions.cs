using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace CallAutomation.AspNetCore.Authentication.JwtBearer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAcsWebHookAuthentication(this IServiceCollection services, Action<AcsJwtBearerOptions> options, string policyName = AcsOpenIdDefaults.PolicyName)
    {
        var acsOpenIdConfiguration = new AcsJwtBearerOptions();
        options(acsOpenIdConfiguration);

        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            acsOpenIdConfiguration.OpenIdConfigurationUrl, new OpenIdConnectConfigurationRetriever());

        services.AddAuthentication()
            .AddJwtBearer(AcsOpenIdDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                var openIdConnectConfiguration = configurationManager.GetConfigurationAsync().Result;

                jwtBearerOptions.Configuration = openIdConnectConfiguration;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = acsOpenIdConfiguration.ValidAudience
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(policyName, policy =>
            {
                policy.AddAuthenticationSchemes(AcsOpenIdDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });

        return services;
    }
}