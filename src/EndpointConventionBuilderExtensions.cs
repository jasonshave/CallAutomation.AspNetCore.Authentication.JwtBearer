using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

namespace CallAutomation.AspNetCore.Authentication.JwtBearer;

public static class EndpointConventionBuilderExtensions
{
    public static TBuilder RequireAcsWebHookAuthorization<TBuilder>(this TBuilder builder, string policyName = AcsOpenIdDefaults.PolicyName) 
        where TBuilder : IEndpointConventionBuilder
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder.RequireAuthorization(new AuthorizeAttribute(policyName));
    }
}